﻿namespace StockRepository.Entities;

public class StockBatch
{
    public required string ProductBatch { get; set; }
    public decimal QuantityAvailable { get; set; }
    public decimal QuantityAllocated { get; set; }
    public decimal QuantityInProcessing { get; set; }
    public DateTime LastMovementDate { get; set; }
    public DateTime LastAllocationDate { get; set; }
}
