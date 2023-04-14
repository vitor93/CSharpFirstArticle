using System.Text.Json.Serialization;

namespace SharedModels.User.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserTypeEnum
{
    User,
    Admin,
    StockManager
}
