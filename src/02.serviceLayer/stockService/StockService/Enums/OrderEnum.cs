using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockService.StockService.Enums;

/// <summary>
/// Ordering Queries
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderEnum
{
    /// <summary>
    /// ASCENDING - ascending order from smaller to greater
    /// </summary>
    [EnumMember(Value = "ASCENDING")]
    [EnumDataType(typeof(OrderEnum))]
    [Description("ASCENDING")]
    ASCENDING,
    /// <summary>
    /// DESCENDING - descending order from greater to smaller
    /// </summary>
    [EnumMember(Value = "DESCENDING")]
    [EnumDataType(typeof(OrderEnum))]
    [Description("DESCENDING")]
    DESCENDING
}
