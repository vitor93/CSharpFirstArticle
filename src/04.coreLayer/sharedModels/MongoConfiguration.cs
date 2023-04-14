namespace SharedModels;

public class MongoConfiguration
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
    public required string StocksCollectionName { get; set; }
    public required string UsersCollectionName { get; set; }
}