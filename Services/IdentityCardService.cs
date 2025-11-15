using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

namespace ExtractInfoIdentityDocument.Services
{
    public class IdentityCardService : IIdentityCardService
    {
        private readonly IRepository<IdentityCard> _identityCardRepository;

        public IdentityCardService(
            IRepository<IdentityCard> identityCardRepository
            )
        {
            _identityCardRepository = identityCardRepository;
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

        public async Task AddIdentityCard(string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null)
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
                    County = !string.IsNullOrEmpty(city) ? county : String.Empty,
                    Country = !string.IsNullOrEmpty(country) ? country : String.Empty
                };

                await _identityCardRepository.InsertAsync(identityCard);
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task EditIdentityCard(string identityCardId, string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null)
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

                    await _identityCardRepository.UpdateAsync(identityCard);
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
