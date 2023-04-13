using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockService.StockService.Enums;

/// <summary>
/// Ordering Queries
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldToOrderCases
{
    /// <summary>
    /// Order by field CreateDate
    /// </summary>
    [EnumMember(Value = "CREATEDATE")]
    [EnumDataType(typeof(FieldToOrderCases))]
    [Description("CREATEDATE")]
    CREATEDATE,
    /// <summary>
    /// Order by field UpdateDate
    /// </summary>
    [EnumMember(Value = "UPDATEDATE")]
    [EnumDataType(typeof(FieldToOrderCases))]
    [Description("UPDATEDATE")]
    UPDATEDATE,
    /// <summary>
    /// Order by field ProductSku 
    /// </summary>
    [EnumMember(Value = "PRODUCTSKU")]
    [EnumDataType(typeof(FieldToOrderCases))]
    [Description("PRODUCTSKU")]
    PRODUCTSKU,
    /// <summary>
    /// Order by field Total Quantity 
    /// </summary>
    [EnumMember(Value = "TOTALQUANTITY")]
    [EnumDataType(typeof(FieldToOrderCases))]
    [Description("TOTALQUANTITY")]
    TOTALQUANTITY
}
