using StocksApi.Utils.Validation;
using StockService.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StocksApi.Models.Request;

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
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FilterCases Filter { get; set; }
    /// <summary>
    /// Order Orientation
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderEnum? OrderOrientation { get; set; }
    /// <summary>
    /// Field Expression to choose What field is used to order
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FieldToOrderCases? FieldToOrderCase { get; set; }

    ///// <summary>
    ///// Secret it is not supposed to appear
    ///// </summary>
    ////[IgnoreDataMember]
    //[SwaggerSchema(ReadOnly = true)]
    //public string Secret { get; set; }

}
