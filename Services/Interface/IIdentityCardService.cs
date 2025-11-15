using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IIdentityCardService
    {
        Task<IdentityCard> GetIdentityCardById(string IdentityCardId);

        Task<IdentityCard> GetIdentityCardByCnp(string cnp);

        Task<List<IdentityCard>> GetAllIdentityCards();

        Task AddIdentityCard(string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null);

        Task AddIdentityCard(IdentityCard identityCard);

        Task EditIdentityCard(string identityCardId, string cnp = null, string series = null, string firstName = null, string lastName = null, string address = null, string city = null, string county = null, string country = null);

        Task DeleteIdentityCardById(string identityCardId);

        Task DeleteIdentityCardByCnp(string cnp);

        Task DeleteAllIdentityCards();
    }
}
