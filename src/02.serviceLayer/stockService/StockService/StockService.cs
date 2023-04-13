using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SharedMethods.Logging;
using SharedMethods.Mapping;
using SharedModels.Response;
using SharedModels.Response.Enums;
using StockRepository.DataContext;
using StockRepository.Entities;
using StockService.StockService.Constants;
using StockService.StockService.Enums;
using StockService.StockService.Mapping;
using StockService.StockService.Models;
using System.Linq.Expressions;
using System.Text.Json;

namespace StockService.StockService;

public class StockService : IStockService
{
    private readonly IStockContext _stockContext;
    private readonly ILogger<StockService> _logger;

    public StockService(IStockContext stockContext, ILogger<StockService> logger)
    {
        _stockContext = stockContext ?? throw new ArgumentNullException(nameof(stockContext)); ;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
    }

    public async Task<StockDto?> GetStockByProductSku(string productSku)
    {
        Stock? stockDto = await _stockContext.GetStockByProductSku(productSku);
        if(stockDto is null)
        {
            _logger.LogWarning(message: string.Format(WarningMessages.CouldNotFindStockBySku, productSku));
            return null;
        }
        return stockDto.Map();
    }

    public async Task<List<StockDto>?> GetStocks(
        int numberOfItems = 100,
        int page = 1,
        FilterCases filter = FilterCases.DEFAULT,
        OrderEnum orderEnum = OrderEnum.ASCENDING,
        FieldToOrderCases fieldToOrderCases = FieldToOrderCases.CREATEDATE)
    {
        try
        {
            var filterToSearch = await GetFilter(filter);

            var expression = await GetFieldForOrderExpression(fieldToOrderCases);

            SortDefinition<Stock>? orderByToQuery = null;

            if (orderEnum.Equals(OrderEnum.ASCENDING))
            {
                orderByToQuery = Builders<Stock>.Sort.Ascending(expression);
            }
            else
            {
                orderByToQuery = Builders<Stock>.Sort.Descending(expression);
            }

            var dbDTO = await _stockContext.GetStocks(numberOfItems, page, filterToSearch, orderByToQuery);

            if (dbDTO is null)
            {
                _logger.LogWarning(message: WarningMessages.CouldNotListStocks);
                return null;
            }

            return dbDTO.Map();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(GetStocks)));

            return null;
        }
    }

    /// <summary>
    /// Private Method to Get Filter to search in DataContext
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    private static async Task<FilterDefinition<Stock>> GetFilter(FilterCases filter)
    {
        return await Task.Run(() =>
        {
            FilterDefinition<Stock>? filterToReturn = null;

            switch (filter)
            {
                case FilterCases.ISPROCESSING:
                    filterToReturn = Builders<Stock>.Filter.Where(x => x.IsStockInProcessing);
                    break;
                case FilterCases.GETALL:
                    filterToReturn = Builders<Stock>.Filter.Where(x => true);
                    break;
                case FilterCases.GETDELETED:
                    filterToReturn = Builders<Stock>.Filter.Where(x => x.IsDeleted);
                    break;
                case FilterCases.GETALLOCATED:
                    filterToReturn = Builders<Stock>.Filter.Where(x => x.IsStockAllocated);
                    break;
                case FilterCases.GETLOCKED:
                    filterToReturn = Builders<Stock>.Filter.Where(x => x.IsStockLocked);
                    break;
                case FilterCases.DEFAULT:
                    filterToReturn = Builders<Stock>.Filter.Where(x => true && !x.IsDeleted);
                    break;
                default:
                    filterToReturn = Builders<Stock>.Filter.Where(x => true && !x.IsDeleted);
                    break;
            }

            return filterToReturn;
        });
    }

    /// <summary>
    /// Private Method to Get Expression to Order By the field within the expression
    /// </summary>
    /// <param name="fieldToOrderCases"></param>
    /// <returns></returns>
    private static async Task<Expression<Func<Stock, object>>> GetFieldForOrderExpression(FieldToOrderCases fieldToOrderCases)
    {
        return await Task.Run(() =>
        {
            Expression<Func<Stock, object>>? expression = null;

            switch (fieldToOrderCases)
            {
                case FieldToOrderCases.CREATEDATE:
                    expression = x => x.CreateDate;
                    break;
                case FieldToOrderCases.PRODUCTSKU:
                    expression = x => x.ProductSku;
                    break;
                case FieldToOrderCases.TOTALQUANTITY:
                    expression = x => x.TotalQuantityAvailable;
                    break;
                case FieldToOrderCases.UPDATEDATE:
                    expression = x => x.UpdateDate;
                    break;
                default:
                    expression = x => x.CreateDate;
                    break;
            }

            return expression;
        });
    }


    public async Task<GenericResponse> InsertStock(StockDto stocks)
    {
        try
        {
            //Validate if Stock Already Exists
            var checkStock = await GetStockByProductSku(stocks.ProductSku);
            if (checkStock != null)
            {
                return new GenericResponse()
                {
                    IsSuccess = false,
                    ResponseStatus = ResponseStatusEnum.INVALIDSTOCKDUPLICATE
                };
            }
            //Before inserting map object from API Entity DTO to DB Entity
            var stock = stocks.Map();
            if(stock is null)
            {
                return new GenericResponse()
                {
                    IsSuccess = false,
                    ResponseStatus = ResponseStatusEnum.INTERNALERROR
                };
            }
            var result = await _stockContext.InsertStock(stock);

            return new GenericResponse()
            {
                IsSuccess = result,
                ResponseStatus = result ? ResponseStatusEnum.OK : ResponseStatusEnum.ERRORINSERTING
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(InsertStock)));

            return new GenericResponse()
            {
                IsSuccess = false,
                ResponseStatus = ResponseStatusEnum.INTERNALERROR
            };
        }
    }

    public async Task<bool> UpdateStock(StockDto stocks)
    {
        var stock = stocks.Map();
        if(stock is null)
        {
            _logger.LogWarning(
                message: string.Format(WarningMessages.CouldNotUpdateStock,
                    JsonSerializer.Serialize(stocks)
                )
            );
            return false;
        }
        return await _stockContext.UpdateStock(stock);
    }
    
    public async Task<bool> DeleteStock(string productSku)
    {
        Stock? stockDto = await _stockContext.GetStockByProductSku(productSku);
        if (stockDto is null)
        {
            _logger.LogWarning(message: string.Format(WarningMessages.CouldNotFindStockBySku, productSku));
            return false;
        }

        stockDto.IsDeleted = true;
        return await _stockContext.UpdateStock(stockDto);
    }

    public async Task<bool> UpdateStocks(List<StockDto> stocksDto)
    {
        var stocks = stocksDto.Map();
        if(stocks is null)
        {
            _logger.LogWarning(
                message: string.Format(WarningMessages.CouldNotUpdateStocks,
                    JsonSerializer.Serialize(stocksDto)
                )
            );
            return false;
        }
        return await _stockContext.UpdateStocks(stocks);
    }
}
