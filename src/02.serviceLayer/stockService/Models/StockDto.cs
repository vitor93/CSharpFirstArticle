namespace StockService.Models;

public class StockDto
{
    public required string ProductSku { get; set; }
    public List<StockBatchDto> StockBatches { get; set; } = default!;
    public bool IsStockLocked { get; set; }
    public bool IsStockAllocated { get; set; }
    public bool IsStockInProcessing { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
    public decimal TotalQuantityAvailable { get; set; }
}
