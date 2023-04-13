using MongoDB.Driver;
using StockRepository.Entities;

namespace StockRepository.DataContext.Seed;

public class StockContextSeed
{
    public async static void SeedData(IMongoCollection<Stock> stockCollection)
    {
        bool existStocks = stockCollection.Find(x => true).Any();
        if (!existStocks)
        {
            await stockCollection.InsertManyAsync(GetPreConfiguredStocks());
        }
    }

    public static IEnumerable<Stock> GetPreConfiguredStocks()
    {
        return new List<Stock>()
            {
                new Stock()
                {
                    IsDeleted = false,
                    ProductSku = "900HXT",
                    IsStockLocked = false,
                    IsStockAllocated = false,
                    IsStockInProcessing = false,
                    StockBatches = new List<StockBatch>()
                    {
                        new StockBatch()
                        {
                            ProductBatch = "900HXT-12-00-03",
                            QuantityAvailable = 1000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }, new StockBatch()
                        {
                            ProductBatch = "900HXT-12-10-03",
                            QuantityAvailable = 25000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }
                    },
                    TotalQuantityAvailable = 26000,
                    CreateDate = System.DateTime.Now.ToUniversalTime(),
                    UpdateDate = System.DateTime.Now.ToUniversalTime(),
                },new Stock()
                {

                    IsDeleted = false,
                    ProductSku = "1000HXT",
                    IsStockLocked = false,
                    IsStockAllocated = false,
                    IsStockInProcessing = false,
                    StockBatches = new List<StockBatch>()
                    {
                        new StockBatch()
                        {
                            ProductBatch = "1000HXT-12-00-03",
                            QuantityAvailable = 700,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }, new StockBatch()
                        {
                            ProductBatch = "1000HXT-12-10-03",
                            QuantityAvailable = 12000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }
                    },
                    TotalQuantityAvailable = 12700,
                    CreateDate = System.DateTime.Now.ToUniversalTime(),
                    UpdateDate = System.DateTime.Now.ToUniversalTime(),
                },new Stock()
                {

                    IsDeleted = true,
                    ProductSku = "700HXT",
                    IsStockLocked = false,
                    IsStockAllocated = false,
                    IsStockInProcessing = false,
                    StockBatches = new List<StockBatch>()
                    {
                        new StockBatch()
                        {
                            ProductBatch = "700HXT-12-00-03",
                            QuantityAvailable = 1000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }, new StockBatch()
                        {
                            ProductBatch = "700HXT-12-10-03",
                            QuantityAvailable = 25000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }
                    },
                    TotalQuantityAvailable = 26000,
                    CreateDate = System.DateTime.Now.ToUniversalTime(),
                    UpdateDate = System.DateTime.Now.ToUniversalTime(),
                },new Stock()
                {

                    IsDeleted = false,
                    ProductSku = "1200HXT",
                    IsStockLocked = false,
                    IsStockAllocated = false,
                    IsStockInProcessing = false,
                    StockBatches = new List<StockBatch>()
                    {
                        new StockBatch()
                        {
                            ProductBatch = "1200HXT-12-00-03",
                            QuantityAvailable = 1500,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }, new StockBatch()
                        {
                            ProductBatch = "1200HXT-12-10-03",
                            QuantityAvailable = 65000,
                            QuantityInProcessing = 0,
                            QuantityAllocated = 0
                        }
                    },
                    TotalQuantityAvailable = 66500,
                    CreateDate = System.DateTime.Now.ToUniversalTime(),
                    UpdateDate = System.DateTime.Now.ToUniversalTime(),
            }
        };
    }
}
