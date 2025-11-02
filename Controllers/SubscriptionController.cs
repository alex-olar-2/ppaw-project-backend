using ExtractInfoIdentityDocument.Models;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(ILogger<SubscriptionController> logger)
    {
        _logger = logger;
    }

    // GET /Subscription
    [HttpGet]
    public Task<IActionResult> GetAllSubscriptions()
    {
        return null;
    }

    // GET /Subscription/{id}
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetSubscriptionById(Guid id)
    {
        return null;
    }

    [HttpPost]
    public Task<IActionResult> AddSubscription([FromBody] Subscription subscription)
    {
        return null;
    }

    [HttpPut("{id:guid}")]
    public Task<IActionResult> EditSubscription(Guid id)
    {
        return null;
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> DeleteSubscription(Guid id)
    {
        return null;
    }

    [HttpDelete]
    public Task<IActionResult> DeleteAllSubscription()
    {
        return null;
    }
}
