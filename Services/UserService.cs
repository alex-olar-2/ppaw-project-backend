using Data.SDK.Repository.Interface;

using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.IdentityModel.Tokens;

namespace ExtractInfoIdentityDocument.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        private readonly ILogger _logger;

        private readonly IRoleService _roleService;

        private readonly ISubscriptionService _subscriptionService;

        public UserService(
            IRepository<User> userRepository,
            IRoleService roleService,
            ISubscriptionService subscriptionService
            )
        {
            _userRepository = userRepository;
            _roleService = roleService;
            _subscriptionService = subscriptionService;
        }

        public async Task<User> GetUserById(string UserId)
        {
            try
            {
                var User = await _userRepository.GetIncludeThenAsync(x => x.Id == Guid.Parse(UserId), false, null);

                return User;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var Users = await _userRepository.GetAllAsync();

                return Users.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddUser(User user)
        {
            try
            {
                await _userRepository.InsertAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddUser(string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null, bool isVisible = true)
        {
            try
            {
                var user = new User 
                {
                    Email = !string.IsNullOrEmpty(email) ? email : String.Empty,
                    Password = !string.IsNullOrEmpty(password) ? password : String.Empty,
                    Cui = !string.IsNullOrEmpty(cui) ? cui : String.Empty,
                    RoleId = !string.IsNullOrEmpty(roleId) ? Guid.Parse(roleId) : (await _roleService.GetDefaultRole()).Id,
                    SubscriptionId = !string.IsNullOrEmpty(subscriptionId) ? Guid.Parse(subscriptionId) : (await _subscriptionService.GetDefaultSubscription()).Id, // de modificat
                    IsVisible = isVisible
                };

                await _userRepository.InsertAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditUser(User user)
        {
            try
            {
                await _userRepository.UpdateOnlyModifiedFieldsAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EditUser(string userId = null, string email = null, string password = null, string cui = null, string subscriptionId = null, string roleId = null, bool isVisible = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await GetUserById(userId);

                    if (!string.IsNullOrEmpty(password))
                    {
                        user.Password = password;
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        user.Email = email;
                    }

                    if (!string.IsNullOrEmpty(cui))
                    {
                        user.Cui = cui;
                    }

                    if (!string.IsNullOrEmpty(roleId))
                    {
                        user.RoleId = Guid.Parse(roleId);

                        Role role = await _roleService.GetRoleById(roleId);

                        if (role != null)
                        {
                            user.Role = role;
                        }
                    }

                    if (!string.IsNullOrEmpty(subscriptionId))
                    {
                        user.RoleId = Guid.Parse(subscriptionId);

                        // De completat cu subscription
                    }

                    user.IsVisible = isVisible;

                    await _userRepository.UpdateOnlyModifiedFieldsAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteUserById(string userId)
        {
            try
            {
                var user = await GetUserById(userId);

                await _userRepository.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteUser(User user)
        {
            try
            {
                await _userRepository.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAllUsers()
        {
            IList<User> Users = await _userRepository.GetAllAsync();

            await _userRepository.DeleteAsync(Users);
        }
    }
}
