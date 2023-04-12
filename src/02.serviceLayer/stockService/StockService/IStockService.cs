using sharedModels.Response;
using stockService.StockService.Enums;
using stockService.StockService.Models;

namespace stockService.StockService;

public interface IStockService
{
    Task<GenericResponse> InsertStock(StockDto stocks);
    Task<bool> UpdateStock(StockDto stocks);
    Task<bool> DeleteStock(string productSku);
    Task<bool> UpdateStocks(List<StockDto> stocksDto);
    Task<List<StockDto>?> GetStocks(
        int numberOfItems = 100,
        int page = 1,
        FilterCases filter = FilterCases.DEFAULT,
        OrderEnum orderEnum = OrderEnum.ASCENDING,
        FieldToOrderCases fieldToOrderCases = FieldToOrderCases.CREATEDATE);
    Task<StockDto?> GetStockByProductSku(string productSku);
}
