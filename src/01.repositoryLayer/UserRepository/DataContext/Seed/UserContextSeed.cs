using MongoDB.Driver;
using SharedMethods.Encription;
using SharedModels.User.Enum;
using UserRepository.Entities;

namespace UserRepository.DataContext.Seed;

internal class UserContextSeed
{
    public async static void SeedData(IMongoCollection<User> stockCollection)
    {
        bool existStocks = stockCollection.Find(x => true).Any();
        if (!existStocks)
        {
            await stockCollection.InsertManyAsync(GetPreConfiguredUsers());
        }
    }

    public static IEnumerable<User> GetPreConfiguredUsers()
    {
        return new List<User>()
        {
            new User()
            {
                Name = "Test Admin",
                Email = "test.admin@domain.com",
                Password = AesOperation.EncryptString("password"),
                UserType = UserTypeEnum.Admin
            },
            new User()
            {
                Name = "Test User",
                Email = "test.user@domain.com",
                Password = AesOperation.EncryptString("password"),
                UserType = UserTypeEnum.User
            },
            new User()
            {
                Name = "Test Manager",
                Email = "test.stock.manager@domain.com",
                Password = AesOperation.EncryptString("password"),
                UserType = UserTypeEnum.StockManager
            }
        };
    }
}
