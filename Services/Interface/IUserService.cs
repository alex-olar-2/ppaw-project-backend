using ExtractInfoIdentityDocument.Models;

namespace ExtractInfoIdentityDocument.Services.Interface
{
    public interface IUserService
    {
        Task<User> GetUserById(string userId);

        Task<List<User>> GetAllUsers();

        Task AddUser(User user);

        Task AddUser(string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null);

        Task EditUser(User user);

        Task EditUser(string userId = null, string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null);

        Task DeleteUser(User user);

        Task DeleteUserById(string userId);

        Task DeleteAllUsers();
    }
}
