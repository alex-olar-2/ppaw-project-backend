using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetSubscriptionById(string subscriptionId);

        Task<Subscription> GetDefaultSubscription();

        Task<List<Subscription>> GetAllSubscriptions();

        Task AddSubscription(string subscriptionName, decimal price, bool isDefault);

        Task AddSubscription(Subscription subscription);

        Task EditSubscription(string subscriptionId, string subscriptionName, decimal price, bool isDefault);

        Task DeleteSubscriptionById(string subscriptionId);

        Task DeleteAllSubscriptions();
    }
}
