using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using System.Diagnostics.Metrics;
using System.Net;

namespace ExtractInfoIdentityDocument.Services
{
    public class UseService : IUseService
    {
        private readonly IRepository<Use> _useRepository;

        public UseService(
            IRepository<Use> useRepository
            )
        {
            _useRepository = useRepository;
        }
        public async Task<Use> GetUseById(string useId)
        {
            try
            {
                Use use = await _useRepository.GetIncludeThenAsync(x => x.Id == Guid.Parse(useId), false, null);

                return use;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Use> GetUseByUserId(string userId)
        {
            try
            {
                Use use = await _useRepository.GetIncludeThenAsync(x => x.UserId == Guid.Parse(userId), false, null);

                return use;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Use> GetUseByIdentityCardId(string identityCardId)
        {
            try
            {
                Use use = await _useRepository.GetIncludeThenAsync(x => x.IdentityCardId == Guid.Parse(identityCardId), false, null);

                return use;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Use>> GetAllUses()
        {
            try
            {
                IList<Use> uses = await _useRepository.GetAllAsync();

                return uses.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddUse(bool isSucceeded, string userId = null, string identityCardId = null)
        {
            try
            {
                Use use = new Use()
                {
                    IsSucceeded = isSucceeded,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserId = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.NewGuid(),
                    IdentityCardId = !string.IsNullOrEmpty(identityCardId) ? Guid.Parse(identityCardId) : Guid.NewGuid(),
                };

                await _useRepository.InsertAsync(use);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddUse(Use Use)
        {
            try
            {
                await _useRepository.InsertAsync(Use);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task EditUse(bool isSucceeded, string useId, string userId = null, string identityCardId = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(useId))
                {
                    Use use = await GetUseById(useId);

                    use.ModifiedAt = DateTime.Now;
                    use.IsSucceeded = isSucceeded != use.IsSucceeded ? isSucceeded : use.IsSucceeded;
                    use.UserId = !string.IsNullOrEmpty(useId) ? Guid.Parse(userId) : use.UserId;
                    use.IdentityCardId = !string.IsNullOrEmpty(identityCardId) ? Guid.Parse(identityCardId) : use.IdentityCardId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteUseById(string useId)
        {
            try
            {
                Use use = await GetUseById(useId);

                await _useRepository.DeleteAsync(use);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteUseByUserId(string userId)
        {
            try
            {
                Use use = await GetUseByUserId(userId);

                await _useRepository.DeleteAsync(use);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteUseByIdentityCardId(string identityCardId)
        {
            try
            {
                Use use = await GetUseByIdentityCardId(identityCardId);

                await _useRepository.DeleteAsync(use);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteAllUses()
        {
            try
            {
                IList<Use> uses = await _useRepository.GetAllAsync();

                await _useRepository.DeleteAsync(uses);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
