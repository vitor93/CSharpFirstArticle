using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using sharedMethods.Logging;
using sharedModels;
using stockRepository.DataContext.Seed;
using stockRepository.Entities;

namespace stockRepository.DataContext;

public class StockContext : IStockContext
{
    private IMongoCollection<Stock> StockContextCollection { get; }
    private readonly ILogger<StockContext> _logger;
    private readonly MongoConfiguration _mongoDbConfig;

    public StockContext(
        IOptions<MongoConfiguration> options,
        ILogger<StockContext> logger)
    {
        try
        {
            _mongoDbConfig = options.Value;
            _logger = logger;

            var client = new MongoClient(_mongoDbConfig.ConnectionString);
            var database = client.GetDatabase(_mongoDbConfig.DatabaseName);
            StockContextCollection = database.GetCollection<Stock>(_mongoDbConfig.StocksCollectionName);

            StockContextSeed.SeedData(StockContextCollection);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> InsertStock(Stock stocks)
    {
        try
        {
            await StockContextCollection.InsertOneAsync(stocks);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(InsertStock)));
            return false;
        }
    }

    public async Task<bool> InsertStocks(List<Stock> stocks)
    {
        try
        {
            await StockContextCollection.InsertManyAsync(stocks);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(InsertStocks)));
            return false;
        }
    }

    public async Task<bool> UpdateStock(Stock stocks)
    {
        try
        {
            var filter = Builders<Stock>.Filter.Eq(p => p.ProductSku, stocks.ProductSku);
            var result = await StockContextCollection.ReplaceOneAsync(filter, stocks);

            return (result != null && result.IsAcknowledged);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(UpdateStock)));
            return false;
        }
    }

    public async Task<bool> UpdateStocks(List<Stock> stocks)
    {
        try
        {
            var dictionaryOfStocks = stocks.ToDictionary(x => x.ProductSku, x => x);
            var checkMultipleReplaces = true;
            foreach (var stock in dictionaryOfStocks)
            {
                var filter = Builders<Stock>.Filter.Eq(p => p.ProductSku, stock.Key);
                var result = await StockContextCollection.ReplaceOneAsync(filter, stock.Value);

                if ((result != null && !result.IsAcknowledged) || result == null)
                {
                    checkMultipleReplaces = false;
                    var message = string.Format("Error updating stock in batch, product sku {0}.", stock.Key);
                    _logger.LogError(message);
                    break;
                }
            }

            return checkMultipleReplaces;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(UpdateStocks)));
            return false;
        }
    }

    public async Task<List<Stock>?> GetStocks(
        int numberOfItems = 100,
        int page = 1,
        FilterDefinition<Stock>? filter = null,
        SortDefinition<Stock>? orderBy = null)
    {
        try
        {
            //validation of page number
            if (page < 1)
            {
                page = 1;
            }
            //Validation of number of items
            if (numberOfItems > 100)
            {
                numberOfItems = 100;
            }

            //Validation of filter
            if (filter == null)
            {
                filter = Builders<Stock>.Filter.Where(x => true && !x.IsDeleted);
            }

            var countFacet = AggregateFacet.Create("count",
               PipelineDefinition<Stock, AggregateCountResult>.Create(new[]
               {
                        PipelineStageDefinitionBuilder.Count<Stock>()
               }));

            //If Orders is not set 
            if (orderBy == null)
            {
                orderBy = Builders<Stock>.Sort.Descending(x => x.CreateDate);
            }

            //Aggregate Creation
            var dataFacet = AggregateFacet.Create("data",
            PipelineDefinition<Stock, Stock>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(orderBy),
                    PipelineStageDefinitionBuilder.Skip<Stock>((page - 1) * numberOfItems),
                    PipelineStageDefinitionBuilder.Limit<Stock>(numberOfItems),
            }));

            var myFilteredStocksPage = await StockContextCollection
                .Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var data = myFilteredStocksPage.First()
                    .Facets.First(x => x.Name == "data")
                    .Output<Stock>();

            return data.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(GetStocks)));
            return null;
        }
    }

    public async Task<Stock?> GetStockByProductSku(string productSku)
    {
        try
        {
            Stock? stock = null;

            if (!string.IsNullOrWhiteSpace(productSku))
            {
                stock = await (await StockContextCollection.FindAsync(x => x.ProductSku.Trim() == productSku.Trim())).FirstOrDefaultAsync();
            }

            return stock;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(GetStockByProductSku)));
            return null;
        }
    }
}
