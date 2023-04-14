using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StocksApi.Models.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService;
using UserService.Models;

namespace StocksApi.Controllers;

/// <summary>
/// Authenticate Controller To do Authentication
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public class AuthenticateController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly ILogger<AuthenticateController> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Authenticate controller to do authentication constructor
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public AuthenticateController(IUserService userService,
        ILogger<AuthenticateController> logger,
        IConfiguration configuration)
    {
        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Generate token valid for 2 hours
    /// </summary>
    /// <returns></returns>
    private string GenerateJSONWebToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
          _configuration["Jwt:Issuer"],
          authClaims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);

        return string.Format("Bearer {0}", new JwtSecurityTokenHandler().WriteToken(token));
    }

    /// <summary>
    /// AutenticateUser
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    private async Task<UserDto?> AuthenticateUser(LoginModel login)
    {
        return await _userService.ValidateUser(login.Email, login.Password);
    }

    /// <summary>  
    /// Login Authenticaton using JWT Token Authentication  
    /// </summary>  
    /// <param name="data"></param>  
    /// <returns></returns>  
    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login([FromForm] LoginModel data)
    {

        if (data == null ||
            (data != null && string.IsNullOrWhiteSpace(data.Email) && string.IsNullOrWhiteSpace(data.Password)))
        {
            return BadRequest();
        }

        ActionResult response = Unauthorized();
        var user = await AuthenticateUser(data!);
        if (data != null && user != null)
        {
            var tokenString = GenerateJSONWebToken(user);
            response = Ok(new { Token = tokenString });
        }
        return response;
    }
}
