using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Server.IISIntegration;

namespace ExtractInfoIdentityDocument.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IRepository<Subscription> _subscriptionRepository;

        private readonly IFileLoggingService _loggingService;

        public SubscriptionService(
            IRepository<Subscription> subscriptionRepository,
            IFileLoggingService loggingService
            ) 
        {
            _subscriptionRepository = subscriptionRepository;
            _loggingService = loggingService;
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

        public async Task AddSubscription(string subscriptionName, decimal price, bool isDefault, bool isVisible = true)
        {
            try
            {
                Subscription subscription = new Subscription { Name = subscriptionName, Price = price, IsDefault = isDefault, IsVisible = isVisible };

                await _subscriptionRepository.InsertAsync(subscription);

                await _loggingService.LogActionAsync("CREATE", "Subscription", $"A fost creat abonamentul cu Id: {subscription.Id} si Nume: {subscription.Name}");
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


                await _loggingService.LogActionAsync("CREATE", "Subscription", $"A fost creat abonamentul cu Id: {subscription.Id} si Nume: {subscription.Name}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // În fișierul: Services/SubscriptionService.cs

        public async Task EditSubscription(string subscriptionId, string subscriptionName, decimal price, bool isDefault, bool isVisible = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(subscriptionId))
                {
                    // 1. Obținem abonamentul existent din baza de date
                    Subscription subscription = await GetSubscriptionById(subscriptionId);

                    if (subscription == null)
                    {
                        throw new Exception("Abonamentul nu a fost găsit.");
                    }

                    // 2. Actualizăm Numele (doar dacă s-a furnizat unul nou)
                    subscription.Name = !string.IsNullOrEmpty(subscriptionName) ? subscriptionName : subscription.Name;

                    // 3. Actualizăm Prețul
                    // Notă: Logica actuală ignoră prețul 0. Dacă dorești abonamente gratuite, șterge condiția `price > 0`.
                    subscription.Price = price > 0 ? price : subscription.Price;

                    // 4. Gestionare logică "IsDefault" (pentru a evita eroarea de index unic SQL)
                    // Dacă setăm acest abonament ca Implicit (true) și el nu era deja implicit...
                    if (isDefault && !subscription.IsDefault)
                    {
                        // ...căutăm vechiul abonament implicit
                        var oldDefault = await GetDefaultSubscription();

                        // Dacă există unul și este diferit de cel curent, îl dezactivăm
                        if (oldDefault != null && oldDefault.Id != subscription.Id)
                        {
                            oldDefault.IsDefault = false;
                            await _subscriptionRepository.UpdateAsync(oldDefault);
                        }
                    }

                    // Setăm noua valoare pentru IsDefault
                    subscription.IsDefault = isDefault;

                    // 5. FIX CRITIC: Atribuim corect IsVisible (înainte era subscription.IsDefault = isVisible)
                    subscription.IsVisible = isVisible;

                    // 6. Salvăm modificările în baza de date
                    await _subscriptionRepository.UpdateAsync(subscription);

                    await _loggingService.LogActionAsync("UPDATE", "Subscription", $"A fost editat abonamentul cu Id: {subscription.Id} si Nume: {subscription.Name}");
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

                await _loggingService.LogActionAsync("DELETE", "Subscription", $"A fost sters abonamentul cu Id: {subscription.Id} si Nume: {subscription.Name}");
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

                await _loggingService.LogActionAsync("DELETE", "Subscription", $"A fost sterse toate abonamentele");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
