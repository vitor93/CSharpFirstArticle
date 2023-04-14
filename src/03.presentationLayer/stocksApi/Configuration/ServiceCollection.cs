using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedConstants;
using SharedModels;
using StockRepository.DataContext;
using StockService;
using System.Text;
using UserRepository.DataContext;
using UserService;

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
        services.AddSingleton<IStockService, StockService.StockService>();
    }
    
    public static void AddUserService(this WebApplicationBuilder webApplicationBuilder)
    {
        var services = webApplicationBuilder.Services;

        services.AddSingleton<IUserContext, UserContext>();
        services.AddSingleton<IUserService, UserService.UserService>();
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

            string? projectName = typeof(ServiceCollection).Namespace!.Split('.').FirstOrDefault();
            var xmlFilename = $"{projectName}.xml";
            var filePath = Path.Combine(System.AppContext.BaseDirectory, xmlFilename);
            config.IncludeXmlComments(filePath, true);

            config.EnableAnnotations();

            //Enable Security Definitions for JWT
            config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });
            //Enable Security Requirement
            config.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });
    }

    public static void AddAuthentication(this WebApplicationBuilder webApplicationBuilder)
    {
        var configuration = webApplicationBuilder.Configuration;

        webApplicationBuilder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });
    }
}
