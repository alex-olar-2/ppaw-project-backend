using ExtractInfoIdentityDocument.Models;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class IdentityCardController : ControllerBase
{
    private readonly ILogger<IdentityCardController> _logger;

    public IdentityCardController(ILogger<IdentityCardController> logger)
    {
        _logger = logger;
    }

    // GET /IdentityCard
    [HttpGet]
    public Task<IActionResult> GetAllIdentityCards()
    {
        return null;
    }

    // GET /IdentityCard/{id}
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetIdentityCardById(Guid id)
    {
        return null;
    }

    [HttpPost]
    public Task<IActionResult> AddIdentityCard([FromBody] IdentityCard identityCard)
    {
        return null;
    }

    [HttpPut("{id:guid}")]
    public Task<IActionResult> EditIdentityCard(Guid id)
    {
        return null;
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteIdentityCard(Guid id)
    {
        return null;
    }

    [HttpDelete]
    public Task<IActionResult> DeleteAllIdentityCard()
    {
        return null;
    }
}
