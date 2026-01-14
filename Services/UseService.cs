using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.Intrinsics.X86;

namespace ExtractInfoIdentityDocument.Services
{
    public class UseService : IUseService
    {
        private readonly IRepository<Use> _useRepository;

        private readonly IFileLoggingService _loggingService;

        public UseService(
            IRepository<Use> useRepository,
            IFileLoggingService loggingService
            )
        {
            _useRepository = useRepository;
            _loggingService = loggingService;
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

        public async Task<List<Use>> GetUsesByUserId(string userId)
        {
            try
            {
                // Obținem toate utilizările
                // NOTĂ: Ideal ar fi să folosim o metodă din Repository care face filtrarea direct în baza de date (ex: FindByCondition)
                // și care face Include la IdentityCard. 
                // Pentru moment, filtrăm lista completă (in-memory filtering):
                IList<Use> allUses = await _useRepository.GetAllAsync();

                var userUses = allUses
                    .Where(x => x.UserId == Guid.Parse(userId))
                    .OrderByDescending(x => x.CreatedAt) // Ordonăm descrescător după dată
                    .ToList();

                return userUses;
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

        public async Task AddUse(bool isSucceeded, string userId = null, string identityCardId = null, bool isVisible = true)
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
                    IsVisible = isVisible
                };

                await _useRepository.InsertAsync(use);

                await _loggingService.LogActionAsync("CREATE", "Use", $"A fost creat utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddUse(Use use)
        {
            try
            {
                await _useRepository.InsertAsync(use);

                await _loggingService.LogActionAsync("CREATE", "Use", $"A fost creat utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task EditUse(bool isSucceeded, string useId, string userId = null, string identityCardId = null, bool isVisible = true)
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
                    use.IsVisible = isVisible;


                    await _loggingService.LogActionAsync("UPDATE", "Use", $"A fost editate utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");
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

                await _loggingService.LogActionAsync("DELETE", "Use", $"A fost sters utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");
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

                await _loggingService.LogActionAsync("DELETE", "Use", $"A fost sters utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");
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

                await _loggingService.LogActionAsync("DELETE", "Use", $"A fost sters utilizarea cu UserId: {use.User} si IdentityCardId: {use.IdentityCardId}");
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

                await _loggingService.LogActionAsync("DELETE", "Use", $"A fost sterse toate utilizarile");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
