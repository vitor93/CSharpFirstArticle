using MongoDB.Driver;
using stockRepository.Entities;

namespace stockRepository.DataContext;

public interface IStockContext
{
    Task<bool> InsertStock(Stock stocks);
    Task<bool> InsertStocks(List<Stock> stocks);
    Task<bool> UpdateStock(Stock stocks);
    Task<bool> UpdateStocks(List<Stock> stocks);
    Task<List<Stock>?> GetStocks(
        int numberOfItems = 100,
        int page = 1,
        FilterDefinition<Stock>? filter = null,
        SortDefinition<Stock>? orderBy = null);

    Task<Stock?> GetStockByProductSku(string productSku);
}
