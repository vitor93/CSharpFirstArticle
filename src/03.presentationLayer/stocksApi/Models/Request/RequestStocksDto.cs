using stocksApi.Utils.Validation;
using stockService.StockService.Enums;
using System.ComponentModel.DataAnnotations;

namespace stocksApi.Models.Request;

/// <summary>
/// Request Stocks DTO
/// </summary>
[ValidateRequestStocksDto]
public class RequestStocksDTO
{
    /// <summary>
    /// Number of items to get
    /// </summary>
    [Required]
    public int NumberOfItems { get; set; }
    /// <summary>
    /// Number of Page
    /// </summary>
    [Required]
    public int Page { get; set; }
    /// <summary>
    /// Filter Case
    /// </summary>
    public FilterCases Filter { get; set; }
    /// <summary>
    /// Order Orientation
    /// </summary>
    public OrderEnum? OrderOrientation { get; set; }
    /// <summary>
    /// Field Expression to choose What field is used to order
    /// </summary>
    public FieldToOrderCases? FieldToOrderCase { get; set; }

    ///// <summary>
    ///// Secret it is not supposed to appear
    ///// </summary>
    ////[IgnoreDataMember]
    //[SwaggerSchema(ReadOnly = true)]
    //public string Secret { get; set; }

}
