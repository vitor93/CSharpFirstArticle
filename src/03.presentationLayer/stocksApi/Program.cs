using StocksApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add Configurations for MongoDB
builder.AddMongoConfigurations();
// Add Stock Service
builder.AddStockServices();
// Add UserService
builder.AddUserService();
// Add Swagger UI
builder.AddSwaggerConfiguration();
// Add Authentication
builder.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
