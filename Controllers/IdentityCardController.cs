using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class IdentityCardController : ControllerBase
{
    private readonly ILogger<IdentityCardController> _logger;
    private readonly IIdentityCardService _identityCardService;

    public IdentityCardController(
        ILogger<IdentityCardController> logger,
        IIdentityCardService identityCardService
        )
    {
        _logger = logger;
        _identityCardService = identityCardService;
    }

    // GET /IdentityCard
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllIdentityCards()
    {
        List<IdentityCard> identityCards = await _identityCardService.GetAllIdentityCards();

        return Ok(identityCards);
    }

    // GET /IdentityCard/{cnp}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetIdentityCardById(string identitiyCardId)
    {
        IdentityCard identityCard = await _identityCardService.GetIdentityCardById(identitiyCardId);

        return Ok(identityCard);
    }

    // GET /IdentityCard/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetIdentityCardByCnp(string cnp)
    {
        IdentityCard identityCard = await _identityCardService.GetIdentityCardByCnp(cnp);

        return Ok(identityCard);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> AddIdentityCard(string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null, bool isVisible = true)
    {
        await _identityCardService.AddIdentityCard(cnp, series, firstName, lastName, address, city, county, country);

        return Ok();
    }

    [Route("[action]")]
    [HttpPut]
    public async Task<IActionResult> EditIdentityCard(string identityCardId = null, string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null, bool isVisible = true)
    {
        await _identityCardService.EditIdentityCard(identityCardId, cnp, series, firstName, lastName, address, city, county, country);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteIdentityCardById(string id)
    {
        await _identityCardService.DeleteIdentityCardById(id);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteIdentityCardByCnp(string cnp)
    {
        await _identityCardService.DeleteIdentityCardByCnp(cnp);

        return Ok();
    }

    [Route("[action]")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllIdentityCard()
    {
        await _identityCardService.DeleteAllIdentityCards();

        return Ok();
    }
}
