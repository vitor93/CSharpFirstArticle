using Microsoft.OpenApi.Models;
using SharedConstants;
using SharedModels;
using StockRepository.DataContext;
using StockService.StockService;

namespace StocksApi.Configuration;

public static class ServiceCollection
{
    public static void AddMongoConfigurations(this WebApplicationBuilder webApplicationBuilder)
    {
        var services = webApplicationBuilder.Services;
        var configuration = webApplicationBuilder.Configuration;

        services.Configure<MongoConfiguration>(configuration.GetSection(Configurations.MongoConfiguration.ToString()));
    }

    public static void AddStockServices(this WebApplicationBuilder webApplicationBuilder)
    {
        var services = webApplicationBuilder.Services;

        services.AddSingleton<IStockContext, StockContext>();
        services.AddSingleton<IStockService, StockService.StockService.StockService>();
    }

    public static void AddSwaggerConfiguration(this WebApplicationBuilder webApplicationBuilder)
    {
        var services = webApplicationBuilder.Services;

        //Add Controllers suppressing Async Suffix In Action Names (we want to keep the async in the methods of our API)
        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Stocks",
                Version = "v1",
                Description = "Stocks",
                Contact = new OpenApiContact
                {
                    Name = "Vitor Craveiro",
                    Email = "v.craveiro@kigroup.de",
                    Url = new Uri("https://www.linkedin.com/in/vitor-craveiro-abb55b97/")
                }
            });

            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var filePath = Path.Combine(System.AppContext.BaseDirectory, xmlFilename);
            config.IncludeXmlComments(filePath, true);

            config.EnableAnnotations();
        });
    }
}
