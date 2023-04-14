using UserService.Models;

namespace UserService;

public interface IUserService
{
    Task<bool> CreateUser(UserDto userdto);
    Task<UserDto?> GetUserByEmail(string email);
    Task<bool> SaveUser(UserDto userDto);
    Task<bool> DeleteUser(string id);
    Task<UserDto?> ValidateUser(string email,
        string password);
}
