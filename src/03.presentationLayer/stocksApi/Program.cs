using stocksApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Configurations for MongoDB
builder.AddMongoConfigurations();
// Add Stock Service
builder.AddStockServices();
// Add Swagger UI
builder.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
