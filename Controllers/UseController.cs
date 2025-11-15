using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UseController : ControllerBase
{
    private readonly ILogger<UseController> _logger;
    private readonly IUseService _useService;

    public UseController(
        ILogger<UseController> logger,
        IUseService useService
        )
    {
        _logger = logger;
        _useService = useService;
    }

    // GET /Use
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllUses()
    {
        List<Use> uses = await _useService.GetAllUses();

        return Ok(uses);
    }

    // GET /Use/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetUseById(string useId)
    {
        Use use = await _useService.GetUseById(useId);

        return Ok(use);
    }

    // GET /Use/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetUseByUserId(string userId)
    {
        Use use = await _useService.GetUseByUserId(userId);

        return Ok(use);
    }

    // GET /Use/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetUseByIdentityCardId(string identityCardId)
    {
        Use use = await _useService.GetUseByIdentityCardId(identityCardId);

        return Ok(use);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AddUse(bool isSucceeded, string userId = null, string identityCardId = null)
    {
        await _useService.AddUse(isSucceeded, userId, identityCardId);

        return Ok();
    }

    [Route("[action]")]
    [HttpPut]
    public async Task<IActionResult> EditUse(bool isSucceeded, string useId, string userId = null, string identityCardId = null)
    {
        await _useService.EditUse(isSucceeded, useId, userId, identityCardId);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUseById(string useId)
    {
        await _useService.DeleteUseById(useId);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUseByUserId(string userId)
    {
        await _useService.DeleteUseByUserId(userId);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUseByIdentityCardId(string identityCardId)
    {
        await _useService.DeleteUseByIdentityCardId(identityCardId);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllUses()
    {
        await _useService.DeleteAllUses();

        return Ok();
    }
}
