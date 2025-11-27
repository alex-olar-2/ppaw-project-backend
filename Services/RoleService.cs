using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

namespace ExtractInfoIdentityDocument.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(
            IRepository<Role> roleRepository
            )
        {
            _roleRepository = roleRepository;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
