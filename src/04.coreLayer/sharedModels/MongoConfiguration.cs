﻿namespace sharedModels;

public class MongoConfiguration
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
    public required string StocksCollectionName { get; set; }
}