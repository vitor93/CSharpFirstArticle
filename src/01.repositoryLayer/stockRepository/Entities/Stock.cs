using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace stockRepository.Entities;

public class Stock
{
    [JsonIgnore]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public required string ProductSku { get; set; }
    public List<StockBatch> StockBatches { get; set; } = default!;
    public bool IsStockLocked { get; set; }
    public bool IsStockAllocated { get; set; }
    public bool IsStockInProcessing { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
    public decimal TotalQuantityAvailable { get; set; }
}
