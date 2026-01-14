using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using System.Data;

namespace ExtractInfoIdentityDocument.Services
{
    public class IdentityCardService : IIdentityCardService
    {
        private readonly IRepository<IdentityCard> _identityCardRepository;
        private readonly IFileLoggingService _loggingService;
        // [MODIFICARE] Adăugăm serviciile necesare pentru Use și UserContext
        private readonly IUseService _useService;
        private readonly IUserContextService _userContextService;

        public IdentityCardService(
            IRepository<IdentityCard> identityCardRepository,
            IFileLoggingService loggingService,
            // [MODIFICARE] Injectăm serviciile
            IUseService useService,
            IUserContextService userContextService
            )
        {
            _identityCardRepository = identityCardRepository;
            _loggingService = loggingService;
            _useService = useService;
            _userContextService = userContextService;
        }

        public async Task<IdentityCard> GetIdentityCardById(string identityCardId)
        {
            try
            {
                IdentityCard identityCard = await _identityCardRepository.GetIncludeThenAsync(x => x.Id == Guid.Parse(identityCardId), false, null);

                return identityCard;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IdentityCard> GetIdentityCardByCnp(string cnp)
        {
            try
            {
                IdentityCard identityCard = await _identityCardRepository.GetIncludeThenAsync(x => x.Cnp == cnp, false, null);

                return identityCard;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<IdentityCard>> GetAllIdentityCards()
        {
            try
            {
                IList<IdentityCard> identityCards = await _identityCardRepository.GetAllAsync();

                return identityCards.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddIdentityCard(string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null, bool isVisible = true)
        {
            try
            {
                IdentityCard identityCard = new IdentityCard()
                {
                    Cnp = !string.IsNullOrEmpty(cnp) ? cnp : String.Empty,
                    Series = !string.IsNullOrEmpty(series) ? series : String.Empty,
                    FirstName = !string.IsNullOrEmpty(firstName) ? firstName : String.Empty,
                    LastName = !string.IsNullOrEmpty(lastName) ? lastName : String.Empty,
                    Address = !string.IsNullOrEmpty(address) ? address : String.Empty,
                    City = !string.IsNullOrEmpty(city) ? city : String.Empty,
                    County = !string.IsNullOrEmpty(county) ? county : String.Empty,
                    Country = !string.IsNullOrEmpty(country) ? country : String.Empty,
                    IsVisible = isVisible
                };

                await _identityCardRepository.InsertAsync(identityCard);

                // [MODIFICARE] Creăm automat un Use după inserare
                try
                {
                    // Obținem ID-ul utilizatorului curent
                    var userId = _userContextService.GetCurrentUserId();

                    // Creăm înregistrarea Use (isSucceeded = true, userId, identityCardId, isVisible)
                    await _useService.AddUse(true, userId.ToString(), identityCard.Id.ToString(), true);
                }
                catch (Exception ex)
                {
                    // Logăm eroarea dacă nu reușim să creăm Use-ul (de ex. user neautentificat), 
                    // dar nu oprim procesul principal
                    await _loggingService.LogActionAsync("ERROR", "Use", $"Nu s-a putut crea Use pentru cardul {identityCard.Id}: {ex.Message}");
                }

                await _loggingService.LogActionAsync("CREATE", "IdentityCard", $"A fost creat card de identitate cu CNP: {identityCard.Cnp} si Id: {identityCard.Id}");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddIdentityCard(IdentityCard identityCard)
        {
            try
            {
                await _identityCardRepository.InsertAsync(identityCard);

                // [MODIFICARE] Adăugăm logica și aici pentru consistență
                try
                {
                    var userId = _userContextService.GetCurrentUserId();
                    await _useService.AddUse(true, userId.ToString(), identityCard.Id.ToString(), true);
                }
                catch (Exception ex)
                {
                    await _loggingService.LogActionAsync("ERROR", "Use", $"Nu s-a putut crea Use pentru cardul {identityCard.Id}: {ex.Message}");
                }

                await _loggingService.LogActionAsync("CREATE", "IdentityCard", $"A fost creat card de identitate cu CNP: {identityCard.Cnp} si Id: {identityCard.Id}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task EditIdentityCard(string identityCardId, string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null, bool isVisible = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(identityCardId))
                {
                    IdentityCard identityCard = await GetIdentityCardById(identityCardId);

                    identityCard.Cnp = !string.IsNullOrEmpty(cnp) ? cnp : identityCard.Cnp;
                    identityCard.Series = !string.IsNullOrEmpty(series) ? series : identityCard.Series;
                    identityCard.FirstName = !string.IsNullOrEmpty(firstName) ? firstName : identityCard.FirstName;
                    identityCard.LastName = !string.IsNullOrEmpty(lastName) ? lastName : identityCard.LastName;
                    identityCard.Address = !string.IsNullOrEmpty(address) ? address : identityCard.Address;
                    identityCard.County = !string.IsNullOrEmpty(county) ? county : identityCard.County;
                    identityCard.Country = !string.IsNullOrEmpty(country) ? country : identityCard.Country;
                    identityCard.IsVisible = isVisible;

                    await _identityCardRepository.UpdateAsync(identityCard);

                    await _loggingService.LogActionAsync("UPDATE", "IdentityCard", $"A fost modificat card de identitate cu CNP: {identityCard.Cnp} si Id: {identityCard.Id}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteIdentityCardById(string identityCardId)
        {
            try
            {
                IdentityCard identityCard = await GetIdentityCardById(identityCardId);

                await _identityCardRepository.DeleteAsync(identityCard);

                await _loggingService.LogActionAsync("DELETE", "IdentityCard", $"A fost sters card de identitate cu CNP: {identityCard.Cnp} si Id: {identityCard.Id}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteIdentityCardByCnp(string cnp)
        {
            try
            {
                IdentityCard identityCard = await GetIdentityCardByCnp(cnp);

                await _identityCardRepository.DeleteAsync(identityCard);

                await _loggingService.LogActionAsync("DELETE", "IdentityCard", $"A fost sters card de identitate cu CNP: {identityCard.Cnp} si Id: {identityCard.Id}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAllIdentityCards()
        {
            try
            {
                IList<IdentityCard> identityCards = await _identityCardRepository.GetAllAsync();

                await _identityCardRepository.DeleteAsync(identityCards);

                await _loggingService.LogActionAsync("DELETE", "IdentityCard", $"Au fost sterse toate cartile de identitate");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
