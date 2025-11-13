using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Server.IISIntegration;

namespace ExtractInfoIdentityDocument.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IRepository<Subscription> _subscriptionRepository;

        public SubscriptionService(
            IRepository<Subscription> subscriptionRepository
            ) 
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<Subscription> GetSubscriptionById(string subscriptionId)
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetIncludeThenAsync(x => x.Id == Guid.Parse(subscriptionId), false, null);

                return subscription;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Subscription> GetDefaultSubscription()
        {
            try
            {
                Subscription subscription = await _subscriptionRepository.GetIncludeThenAsync(x => x.IsDefault == true, false, null);

                return subscription;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            try
            {
                IList<Subscription> subscription = await _subscriptionRepository.GetAllAsync();

                return subscription.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddSubscription(string subscriptionName, decimal price, bool isDefault)
        {
            try
            {
                Subscription subscription = new Subscription { Name = subscriptionName, Price = price, IsDefault = isDefault };

                await _subscriptionRepository.InsertAsync(subscription);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddSubscription(Subscription subscription)
        {
            try
            {
                await _subscriptionRepository.InsertAsync(subscription);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditSubscription(string subscriptionId, string subscriptionName, decimal price, bool isDefault)
        {
            try
            {
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    Subscription subscription = await GetSubscriptionById(subscriptionId);

                    subscription.Name = !string.IsNullOrEmpty(subscriptionName) ? subscriptionName : String.Empty;
                    subscription.Price = price > 0 ? price : subscription.Price;
                    subscription.IsDefault = isDefault != null ? isDefault : false;

                    await _subscriptionRepository.UpdateAsync(subscription);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteSubscriptionById(string subscriptionId)
        {
            try
            {
                Subscription subscription = await GetSubscriptionById(subscriptionId);

                await _subscriptionRepository.DeleteAsync(subscription);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAllSubscriptions()
        {
            try
            {
                IList<Subscription> subscriptions = await _subscriptionRepository.GetAllAsync();

                await _subscriptionRepository.DeleteAsync(subscriptions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
