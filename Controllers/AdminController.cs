using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index()
        {
            return View();
        }

        // ========================== USERS ==========================

        public async Task<IActionResult> Users()
        {
            List<User> users = await _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            ViewBag.Roles = new SelectList(await _roleService.GetAllRoles(), "Id", "Name");
            ViewBag.Subscriptions = new SelectList(await _subscriptionService.GetAllSubscriptions(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string password, string cui, string subscriptionId, string roleId, bool isVisible)
        {
            await _userService.AddUser(email, password, cui, subscriptionId, roleId, isVisible);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound();

            // Verifică dacă aceste linii există și dacă returnează date
            ViewBag.Roles = new SelectList(await _roleService.GetAllRoles(), "Id", "Name", user.RoleId);
            ViewBag.Subscriptions = new SelectList(await _subscriptionService.GetAllSubscriptions(), "Id", "Name", user.SubscriptionId);

            return View(user);
        }

        // POST: Procesează formularul trimis
        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string email, string password, string cui, string subscriptionId, string roleId, bool isVisible)
        {
            // Aici primim ID-urile (subscriptionId și roleId) din formular, nu numele.
            await _userService.EditUser(id, email, password, cui, subscriptionId, roleId, isVisible);

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUserById(id);
            return RedirectToAction(nameof(Users));
        }

        // ========================== ROLES ==========================

        public async Task<IActionResult> Roles()
        {
            List<Role> roles = await _roleService.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name, bool isDefault, bool isVisible)
        {
            await _roleService.AddRole(name, isDefault, isVisible);
            return RedirectToAction(nameof(Roles));
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleService.GetRoleById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string id, string name, bool isDefault, bool isVisible)
        {
            await _roleService.EditRole(id, name, isDefault, isVisible);
            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await _roleService.DeleteRoleById(id);
            return RedirectToAction(nameof(Roles));
        }

        // ========================== SUBSCRIPTIONS ==========================

        public async Task<IActionResult> Subscriptions()
        {
            List<Subscription> subscriptions = await _subscriptionService.GetAllSubscriptions();
            return View(subscriptions);
        }

        [HttpGet]
        public IActionResult CreateSubscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(string name, decimal price, bool isDefault, bool isVisible)
        {
            await _subscriptionService.AddSubscription(name, price, isDefault, isVisible);
            return RedirectToAction(nameof(Subscriptions));
        }

        [HttpGet]
        public async Task<IActionResult> EditSubscription(string id)
        {
            var sub = await _subscriptionService.GetSubscriptionById(id);
            if (sub == null) return NotFound();
            return View(sub);
        }

        [HttpPost]
        public async Task<IActionResult> EditSubscription(string id, string name, decimal price, bool isDefault, bool isVisible)
        {
            await _subscriptionService.EditSubscription(id, name, price, isDefault, isVisible);
            return RedirectToAction(nameof(Subscriptions));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubscription(string id)
        {
            await _subscriptionService.DeleteSubscriptionById(id);
            return RedirectToAction(nameof(Subscriptions));
        }

        // ========================== IDENTITY CARDS ==========================

        public async Task<IActionResult> IdentityCards()
        {
            List<IdentityCard> identityCards = await _identityCardService.GetAllIdentityCards();
            return View(identityCards);
        }

        [HttpGet]
        public IActionResult CreateIdentityCard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIdentityCard(string cnp, string series, string firstName, string lastName, string address, string city, string county, string country, bool isVisible)
        {
            await _identityCardService.AddIdentityCard(cnp, series, firstName, lastName, address, city, county, country, isVisible);
            return RedirectToAction(nameof(IdentityCards));
        }

        [HttpGet]
        public async Task<IActionResult> EditIdentityCard(string id)
        {
            var card = await _identityCardService.GetIdentityCardById(id);
            if (card == null) return NotFound();
            return View(card);
        }

        [HttpPost]
        public async Task<IActionResult> EditIdentityCard(string id, string cnp, string series, string firstName, string lastName, string address, string city, string county, string country, bool isVisible)
        {
            await _identityCardService.EditIdentityCard(id, cnp, series, firstName, lastName, address, city, county, country, isVisible);
            return RedirectToAction(nameof(IdentityCards));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIdentityCard(string id)
        {
            await _identityCardService.DeleteIdentityCardById(id);
            return RedirectToAction(nameof(IdentityCards));
        }

        // ========================== USES (Logs) ==========================

        public async Task<IActionResult> Uses()
        {
            List<Use> uses = await _useService.GetAllUses();
            return View(uses);
        }

        // De obicei Uses sunt log-uri automate, dar am adaugat Delete pentru cleanup
        [HttpPost]
        public async Task<IActionResult> DeleteUse(string id)
        {
            await _useService.DeleteUseById(id);
            return RedirectToAction(nameof(Uses));
        }
    }
}
