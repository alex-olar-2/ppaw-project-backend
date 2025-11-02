using ExtractInfoIdentityDocument.Models;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UseController : ControllerBase
{
    private readonly ILogger<UseController> _logger;

    public UseController(ILogger<UseController> logger)
    {
        _logger = logger;
    }

    // GET /Use
    [HttpGet]
    public Task<IActionResult> GetAllUses()
    {
        return null;
    }

    // GET /Use/{id}
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetUseById(Guid id)
    {
        return null;
    }

    [HttpPost]
    public Task<IActionResult> AddUse([FromBody] Use use)
    {
        return null;
    }

    [HttpPut("{id:guid}")]
    public Task<IActionResult> EditUse(Guid id)
    {
        return null;
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteUse(Guid id)
    {
        return null;
    }

    [HttpDelete]
    public Task<IActionResult> DeleteAllUse()
    {
        return null;
    }
}
