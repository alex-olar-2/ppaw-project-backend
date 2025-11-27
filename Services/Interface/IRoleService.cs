using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IRoleService
    {
        Task<Role> GetRoleById(string roleId);

        Task<Role> GetDefaultRole();

        Task<List<Role>> GetAllRoles();

        Task AddRole(string roleName, bool isDefault, bool isVisible = true);
        
        Task AddRole(Role role);

        Task EditRole(string roleId, string roleName, bool isDefault, bool isVisible = true);

        Task DeleteRoleById(string roleId);

        Task DeleteAllRoles();
    }
}
