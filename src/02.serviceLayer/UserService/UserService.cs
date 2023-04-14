using Microsoft.Extensions.Logging;
using SharedMethods.Encription;
using System.Text.Json;
using UserRepository.DataContext;
using UserService.Constants;
using UserService.Mapping;
using UserService.Models;

namespace UserService;

public class UserService : IUserService
{
    private readonly IUserContext _userContext;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserContext userContext, ILogger<UserService> logger)
    {
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Method to Create User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> CreateUser(UserDto userdto)
    {
        var checkIfUserExists = await _userContext.GetUserByEmail(userdto.Email);

        if (checkIfUserExists != null)
        {
            return false;
        }

        userdto.Password = AesOperation.EncryptString(userdto.Password);

        var user = userdto.Map();

        if (user is null)
        {
            _logger.LogWarning(message: string.Format(WarningMessages.CouldNotCreateUser, JsonSerializer.Serialize(userdto)));
            return false;
        }

        return await _userContext.CreateUser(user);
    }

    /// <summary>
    /// Get User By Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<UserDto?> GetUserByEmail(string email)
    {
        return (await _userContext.GetUserByEmail(email))!.Map();
    }

    /// <summary>
    ///  Method to save user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> SaveUser(UserDto userDto)
    {

        var user = await _userContext.GetUserByEmail(userDto.Email);

        if(user is null)
        {
            return false;
        }

        user.Password = AesOperation.EncryptString(userDto.Password);

        if (user != null)
        {
            return await _userContext.SaveUser(user);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Method to delete user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<bool> DeleteUser(string id)
    {
        var user = await _userContext.GetUserById(id);

        if (user != null)
        {
            return await _userContext.DeleteUser(user);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Validate user email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<UserDto?> ValidateUser(string email,
        string password)
    {

        var user = await _userContext.GetUserByEmail(email);

        if (user != null)
        {
            UserDto? checkPassword = null;
            if (AesOperation.DecryptString(user.Password).Equals(password))
            {
                checkPassword = user.Map()!;
            }
            return checkPassword;
        }
        else
        {
            return null;
        }

    }
}
