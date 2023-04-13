using MongoDB.Driver;
using StockRepository.Entities;

namespace StockRepository.DataContext;

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
