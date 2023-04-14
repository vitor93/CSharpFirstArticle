using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedMethods.Logging;
using SharedModels;
using UserRepository.DataContext.Seed;
using UserRepository.Entities;

namespace UserRepository.DataContext;

public class UserContext : IUserContext
{
    private IMongoCollection<User> UserContextCollection { get; }
    private readonly ILogger<UserContext> _logger;
    private readonly MongoConfiguration _mongoDbConfig;

    public UserContext(
        IOptions<MongoConfiguration> options,
        ILogger<UserContext> logger)
    {
        try
        {
            _mongoDbConfig = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var client = new MongoClient(_mongoDbConfig.ConnectionString);
            var database = client.GetDatabase(_mongoDbConfig.DatabaseName);
            UserContextCollection = database.GetCollection<User>(_mongoDbConfig.UsersCollectionName);

            UserContextSeed.SeedData(UserContextCollection);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Create user in mongo db
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public async Task<bool> CreateUser(User user)
    {
        if (user == null)
        {
            return false;
        }

        try
        {
            await UserContextCollection.InsertOneAsync(user);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(CreateUser)));

            return false;
        }
    }

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> DeleteUser(User user)
    {
        if (user == null)
        {
            return false;
        }

        try
        {
            await UserContextCollection.DeleteOneAsync(x => x.Id.Equals(user.Id));

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(DeleteUser)));

            return false;
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<User?> GetUserByEmail(string email)
    {
        try
        {
            return (await UserContextCollection.FindAsync(x => x.Email.Equals(email))).FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(GetUserByEmail)));
            return null;
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<User?> GetUserById(string id)
    {
        try
        {
            return (await UserContextCollection.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(GetUserById)));
            return null;
        }
    }

    /// <summary>
    /// Save User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> SaveUser(User user)
    {
        try
        {
            var resultFromUpdate = await UserContextCollection.ReplaceOneAsync(x => x.Id.Equals(user.Id), user);

            if (resultFromUpdate != null && resultFromUpdate.IsAcknowledged)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetGenericError(nameof(SaveUser)));
            return false;
        }
    }
}
