using System.ComponentModel.DataAnnotations;

namespace StocksApi.Models.Request;

public class LoginModel
{
    /// <summary>
    /// Email of user
    /// </summary>
    [Required]
    public string Email { get; set; } = default!;

    /// <summary>
    /// Password of user
    /// </summary>
    [Required]
    public string Password { get; set; } = default!;
}
