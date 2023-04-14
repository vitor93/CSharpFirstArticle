using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedModels.User.Enum;
using System.Text.Json.Serialization;

namespace UserRepository.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;
    [BsonRequired]
    public string Name { get; set; } = default!;
    [BsonRequired]
    public string Email { get; set; } = default!;
    [BsonRequired]
    public string Password { get; set; } = default!;
    [BsonRequired]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserTypeEnum UserType { get; set; }

}
