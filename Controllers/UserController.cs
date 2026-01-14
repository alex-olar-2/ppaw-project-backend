using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(
        ILogger<UserController> logger,
        IUserService userService
        )
    {
        _logger = logger;
        _userService = userService;
    }

    // GET /Login
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> Login(string email, string password)
    {
        List<User> Users = await _userService.Login(email, password);

        return Ok(Users);
    }

    // GET /User
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        List<User> Users = await _userService.GetAllUsers();

        return Ok(Users);
    }

    // GET /User/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetUserById(string userId)
    {
        User User = await _userService.GetUserById(userId);

        return Ok(User);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AddUser(string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null, bool isVisible = true)
    {
        await _userService.AddUser(email, password, cui, subscriptionId, roleId);

        return Ok();
    }

    [Route("[action]")]
    [HttpPut]
    public async Task<IActionResult> EditUser(string userId, string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null, bool isVisible = true)
    {
        await _userService.EditUser(userId, email, password, cui, subscriptionId, roleId);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUserById(string id)
    {
        await _userService.DeleteUserById(id);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllUser()
    {
        await _userService.DeleteAllUsers();

        return Ok();
    }
}
