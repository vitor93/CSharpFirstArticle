using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SharedModels.Response.Enums;

/// <summary>
/// Response Status Enum
/// </summary>
public enum ResponseStatusEnum
{
    /// <summary>
    /// OK - Request was ok and had no problems
    /// </summary>
    [EnumMember(Value = "OK")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("OK")]
    OK,
    /// <summary>
    /// ERRORUPDATING - Request had an error while updating
    /// </summary>
    [EnumMember(Value = "ERRORUPDATING")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("ERRORUPDATING")]
    ERRORUPDATING,
    /// <summary>
    /// ERRORINSERTING - Request had an error while inserting
    /// </summary>
    [EnumMember(Value = "ERRORINSERTING")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("ERRORINSERTING")]
    ERRORINSERTING,
    /// <summary>
    /// ERRORFETCHING - Request had an error while trying to fetch data
    /// </summary>
    [EnumMember(Value = "ERRORFETCHING")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("ERRORFETCHING")]
    ERRORFETCHING,
    /// <summary>
    /// INTERNALERROR - Request had an error internally, one problem might be lack of resources to process the request or connection to DataBase
    /// </summary>
    [EnumMember(Value = "INTERNALERROR")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("INTERNALERROR")]
    INTERNALERROR,
    /// <summary>
    /// INVALIDSTOCKDUPLICATE - The stock you are creating already exists
    /// </summary>
    [EnumMember(Value = "INVALIDSTOCKDUPLICATE")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("INVALIDSTOCKDUPLICATE")]
    INVALIDSTOCKDUPLICATE,
    /// <summary>
    /// INVALIDSTOCKDUPLICATE - The stock you are creating already exists
    /// </summary>
    [EnumMember(Value = "ERRORDELETING")]
    [EnumDataType(typeof(ResponseStatusEnum))]
    [Description("ERRORDELETING")]
    ERRORDELETING
}
