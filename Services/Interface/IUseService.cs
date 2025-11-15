using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IUseService
    {
        Task<Use> GetUseById(string useId);

        Task<Use> GetUseByUserId(string userId);

        Task<Use> GetUseByIdentityCardId(string userId);

        Task<List<Use>> GetAllUses();

        Task AddUse(bool isSucceeded, string userId = null, string identityCardId = null);

        Task AddUse(Use Use);

        Task EditUse(bool isSucceeded, string useId, string userId = null, string identityCardId = null);

        Task DeleteUseById(string useId);

        Task DeleteUseByUserId(string userId);

        Task DeleteUseByIdentityCardId(string identityCardId);

        Task DeleteAllUses();
    }
}
