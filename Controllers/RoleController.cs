using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> _logger;

    public RoleController(ILogger<RoleController> logger)
    {
        _logger = logger;
    }

    // GET /Role
    [HttpGet]
    public Task<IActionResult> GetAllRoles()
    {
        return null;
    }

    // GET /Role/{id}
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetRoleById(Guid id)
    {
        return null;
    }

    [HttpPost]
    public Task<IActionResult> AddRole()
    {
        return null;
    }

    [HttpPut("{id:guid}")]
    public Task<IActionResult> EditRole(Guid id)
    {
        return null;
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteRole(Guid id)
    {
        return null;
    }

    [HttpDelete("All")]
    public Task<IActionResult> DeleteAllRole()
    {
        return null;
    }
}
