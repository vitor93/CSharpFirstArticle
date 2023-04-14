using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserService;
using UserService.Models;
using SharedModels.User.Enum;
using StocksApi.Utils.AuthorizationRoles;

namespace StocksApi.Controllers;

/// <summary>
/// User Controller to manage users in interview api
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    /// <summary>
    /// User Controller to manage users in interview api Constructor
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Method to Create User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("[action]", Name = "CreateUser")]
    [HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateUser([FromForm] UserDto user)
    {
        if (user == null || (user != null && (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))))
        {
            return BadRequest("User is not filled.");
        }

        var userToSend = new UserDto()
        {
            Email = user!.Email,
            Name = user.Name,
            UserType = user.UserType,
            Password = user.Password
        };

        var result = await _userService.CreateUser(userToSend);

        if (result)
        {
            return CreatedAtAction(nameof(CreateUser), user);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving User.");
        }
    }

    /// <summary>
    /// Method to Delete User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("[action]", Name = "DeleteUser")]
    [HttpDelete]
    [AuthorizeRoles(UserTypeEnum.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser([FromForm] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("User is not filled.");
        }

        var result = await _userService.DeleteUser(id);

        if (result)
        {
            return Ok(id);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting User.");
        }
    }

    /// <summary>
    /// Method to Edit User
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Route("[action]", Name = "EditUser")]
    [HttpPost]
    [AuthorizeRoles(UserTypeEnum.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> EditUser([FromForm] UserDto user)
    {
        if (user == null || (user != null && (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))))
        {
            return BadRequest("User is not filled.");
        }

        var result = await _userService.SaveUser(user!);

        if (result)
        {
            return CreatedAtAction(nameof(CreateUser), user);
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving User.");
        }
    }

    /// <summary>
    /// Method to Get User by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [Route("[action]", Name = "GetUserByEmail")]
    [HttpGet]
    [AuthorizeRoles(UserTypeEnum.Admin,UserTypeEnum.StockManager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserByEmail([FromHeader] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest();
        }

        var result = await _userService.GetUserByEmail(email);

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound(string.Format("User with email {0} not found", email.Trim()));
        }
    }

}