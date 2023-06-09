﻿using System.ComponentModel.DataAnnotations;

namespace StocksApi.Models.Request;

public class RequestStockDeleteDto
{
    /// <summary>
    /// Product SKU to delete
    /// </summary>
    [Required]
    public required string ProductSku { get; set; }
}
