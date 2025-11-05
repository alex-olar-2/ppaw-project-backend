using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IRoleService
    {
        Task<Role> GetRoleById(string roleId);

        Task<List<Role>> GetAllRoles();

        Task AddRole(string roleName);

        Task EditRole(string roleId, string roleName);

        Task DeleteRoleById(string roleId);

        Task DeleteAllRoles();
    }
}
