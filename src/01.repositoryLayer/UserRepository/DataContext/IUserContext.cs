using UserRepository.Entities;

namespace UserRepository.DataContext;

public interface IUserContext
{
    Task<bool> CreateUser(User user);
    Task<bool> DeleteUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(string id);
    Task<bool> SaveUser(User user);
}
