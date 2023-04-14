using SharedMethods.Mapping;
using UserRepository.Entities;
using UserService.Models;

namespace UserService.Mapping;

internal static class UserMapper
{
    public static User? Map(this UserDto stockDto)
    {
        var destination = Mapper.Map<UserDto, User>(stockDto);
        return destination;
    }

    public static List<User>? Map(this List<UserDto> stockDto)
    {
        var destination = Mapper.MapList<UserDto, User>(stockDto);
        return destination!.ToList();
    }

    public static UserDto? Map(this User stock)
    {
        var destination = Mapper.Map<User, UserDto>(stock);
        return destination;
    }

    public static List<UserDto>? Map(this List<User> stocks)
    {
        var destination = Mapper.MapList<User, UserDto>(stocks);
        return destination!.ToList();
    }
}
