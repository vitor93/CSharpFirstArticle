using SharedModels.User.Enum;

namespace UserService.Models;

public class UserDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserTypeEnum UserType { get; set; }
}
