using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SubscriptionController : ControllerBase
{
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionController(
        ILogger<SubscriptionController> logger,
        ISubscriptionService subscriptionService
        )
    {
        _logger = logger;
        _subscriptionService = subscriptionService;
    }

    // GET /Subscription
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        List<Subscription> subscriptions = await _subscriptionService.GetAllSubscriptions();

        return Ok(subscriptions);
    }

    // GET /Subscription
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetDefaultSubscription()
    {
        Subscription subscription = await _subscriptionService.GetDefaultSubscription();

        return Ok(subscription);
    }

    // GET /Subscription/{id}
    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetSubscriptionById(string id)
    {
        Subscription subscription = await _subscriptionService.GetSubscriptionById(id);

        return Ok(subscription);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> AddSubscription(string subscriptionName, decimal price, bool isDefault, bool isVisible = true)
    {
        await _subscriptionService.AddSubscription(subscriptionName, price, isDefault);

        return Ok();
    }

    [HttpPut]
    [Route("[action]")]
    public async Task<IActionResult> EditSubscription(string subscriptionId, string subscriptionName, decimal price, bool isDefault, bool isVisible = true)
    {
        await _subscriptionService.EditSubscription(subscriptionId, subscriptionName, price, isDefault, isVisible);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task<IActionResult> DeleteSubscriptionById(string subscriptionId)
    {
        await _subscriptionService.DeleteSubscriptionById(subscriptionId);

        return Ok();
    }

    [HttpDelete]
    [Route("[action]")]
    public async Task<IActionResult> DeleteAllSubscription()
    {
        await _subscriptionService.DeleteAllSubscriptions();

        return Ok();
    }
}
