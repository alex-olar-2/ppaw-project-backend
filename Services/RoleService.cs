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

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();

                return roles.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddRole(string roleName)
        {
            try
            {
                var role = new Role { Name = roleName };

                await _roleRepository.InsertAsync(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditRole(string roleId, string roleName)
        {
            try
            {
                var role = await GetRoleById(roleId);

                role.Name = roleName;

                await _roleRepository.UpdateAsync(role);
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
                var role = await GetRoleById(roleId);

                await _roleRepository.DeleteAsync(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAllRoles()
        {
            IList<Role> roles = await _roleRepository.GetAllAsync();

            await _roleRepository.DeleteAsync(roles);
        }
    }
}
