using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockService.Enums;

/// <summary>
/// Filter Cases
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterCases
{
    /// <summary>
    /// ISPROCESSING - Get only stocks in processing
    /// </summary>
    [EnumMember(Value = "ISPROCESSING")]
    [EnumDataType(typeof(FilterCases))]
    [Description("ISPROCESSING")]
    ISPROCESSING,
    /// <summary>
    /// GETALL - Get all stocks
    /// </summary>
    [EnumMember(Value = "GETALL")]
    [EnumDataType(typeof(FilterCases))]
    [Description("GETALL")]
    GETALL,
    /// <summary>
    /// GETDELETED - Get all Deleted stocks
    /// </summary>
    [EnumMember(Value = "GETDELETED")]
    [EnumDataType(typeof(FilterCases))]
    [Description("GETDELETED")]
    GETDELETED,
    /// <summary>
    /// GETALLOCATED - Get all stocks allocated
    /// </summary>
    [EnumMember(Value = "GETALLOCATED")]
    [EnumDataType(typeof(FilterCases))]
    [Description("GETALLOCATED")]
    GETALLOCATED,
    /// <summary>
    /// GETLOCKED - Get all stocks locked
    /// </summary>
    [EnumMember(Value = "GETLOCKED")]
    [EnumDataType(typeof(FilterCases))]
    [Description("GETLOCKED")]
    GETLOCKED,
    /// <summary>
    /// DEFAULT - Get all stocks by default (are not deleted)
    /// </summary>
    [EnumMember(Value = "DEFAULT")]
    [EnumDataType(typeof(FilterCases))]
    [Description("DEFAULT")]
    DEFAULT
}