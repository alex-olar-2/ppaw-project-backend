using ExtractInfoIdentityDocument.Models;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    // GET /User
    [HttpGet]
    public Task<IActionResult> GetAllUsers()
    {
        return null;
    }

    // GET /User/{id}
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetUserById(Guid id)
    {
        return null;
    }

    [HttpPost]
    public Task<IActionResult> AddUser([FromBody] User user)
    {
        return null;
    }

    [HttpPut("{id:guid}")]
    public Task<IActionResult> EditUser(Guid id)
    {
        return null;
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteUser(Guid id)
    {
        return null;
    }

    [HttpDelete]
    public Task<IActionResult> DeleteAllUser()
    {
        return null;
    }
}
