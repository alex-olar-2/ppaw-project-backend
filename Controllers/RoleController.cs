using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly ILogger<RoleController> _logger;
    private readonly IRoleService _roleService;

    public RoleController(
        ILogger<RoleController> logger,
        IRoleService roleService
        )
    {
        _logger = logger;
        _roleService = roleService;
    }

    // GET /Role
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        List<Role> roles = await _roleService.GetAllRoles();

        return Ok(roles);
    }

    // GET /Role/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetRoleById(string roleId)
    {
        Role role = await _roleService.GetRoleById(roleId);

        return Ok(role);
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetDefaultRole()
    {
        Role role = await _roleService.GetDefaultRole();

        return Ok(role);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AddRole(string roleName, bool isDefault)
    {
        await _roleService.AddRole(roleName, isDefault);

        return Ok();
    }

    [Route("[action]")]
    [HttpPut]
    public async Task<IActionResult> EditRole(string roleId, string newName, bool isDefault)
    {
        await _roleService.EditRole(roleId, newName, isDefault);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteRoleById(string id)
    {
        await _roleService.DeleteRoleById(id);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllRole()
    {
        await _roleService.DeleteAllRoles();

        return Ok();
    }
}
