using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using System.Data;

namespace ExtractInfoIdentityDocument.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        private readonly IFileLoggingService _loggingService;

        public RoleService(
            IRepository<Role> roleRepository,
            IFileLoggingService loggingService
            )
        {
            _roleRepository = roleRepository;
            _loggingService = loggingService;
        }

        public async Task<Role> GetRoleById(string roleId)
        {
            try
            {
                var role = await _roleRepository.GetIncludeThenAsync(x => x.Id == Guid.Parse(roleId), false, null);

                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Role> GetDefaultRole()
        {
            try
            {
                Role role = await _roleRepository.GetIncludeThenAsync(x => x.IsDefault == true, false, null);

                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                IList<Role> roles = await _roleRepository.GetAllAsync();

                return roles.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddRole(string roleName, bool isDefault, bool isVisible = true)
        {
            try
            {
                Role role = new Role { Name = roleName, IsDefault = isDefault, IsVisible = isVisible };

                await _roleRepository.InsertAsync(role);

                await _loggingService.LogActionAsync("CREATE", "Role", $"A fost creat rol cu Id: {role.Id} si Nume: {role.Name}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task AddRole(Role role)
        {
            try
            {
                await _roleRepository.InsertAsync(role);

                await _loggingService.LogActionAsync("CREATE", "Role", $"A fost creat rol cu Id: {role.Id} si Nume: {role.Name}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditRole(string roleId, string roleName, bool isDefault, bool isVisible = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    Role role = await GetRoleById(roleId);

                    role.Name = !string.IsNullOrEmpty(roleName) ? roleName : String.Empty;
                    role.IsDefault = isDefault != null ? isDefault : false;
                    role.IsVisible = isVisible;

                    await _roleRepository.UpdateAsync(role);

                    await _loggingService.LogActionAsync("UPDATE", "Role", $"A fost editat rol cu Id: {role.Id} si Nume: {role.Name}");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteRoleById(string roleId)
        {
            try
            {
                Role role = await GetRoleById(roleId);

                await _roleRepository.DeleteAsync(role);

                await _loggingService.LogActionAsync("DELETE", "Role", $"A fost sters rol cu Id: {role.Id} si Nume: {role.Name}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAllRoles()
        {
            try
            {
                IList<Role> roles = await _roleRepository.GetAllAsync();

                await _roleRepository.DeleteAsync(roles);

                await _loggingService.LogActionAsync("DELETE", "Role", $"Au fost sterse toate rolurile");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
