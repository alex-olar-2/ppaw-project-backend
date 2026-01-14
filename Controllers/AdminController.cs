using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExtractInfoIdentityDocument.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IUseService _useService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IIdentityCardService _identityCardService;

        public AdminController(IRoleService roleService, IUserService userService, IUseService useService, ISubscriptionService subscriptionService, IIdentityCardService identityCardService)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            _userService = userService;
            _useService = useService;
            _subscriptionService = subscriptionService;
            _identityCardService = identityCardService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            List<User> users = await _userService.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> Uses()
        {
            List<Use> uses = await _useService.GetAllUses();
            return View(uses);
        }

        public async Task<IActionResult> Subscriptions()
        {
            List<Subscription> subscriptions = await _subscriptionService.GetAllSubscriptions();
            return View(subscriptions);
        }

        public async Task<IActionResult> Roles()
        {
            List<Role> roles = await _roleService.GetAllRoles();
            return View(roles);
        }

        public async Task<IActionResult> IdentityCards()
        {
            List<IdentityCard> identityCards = await _identityCardService.GetAllIdentityCards();
            return View(identityCards);
        }
    }
}
