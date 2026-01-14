using ExtractInfoIdentityDocument.Models;
using ExtractInfoIdentityDocument.Services.Interface;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace ExtractInfoIdentityDocument.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Email și parola sunt obligatorii.";
                return View();
            }

            // 1. Verificăm credențialele
            User user = await _userService.Login(email, password);

            if (user == null)
            {
                ViewBag.Error = "Email sau parolă incorectă.";
                return View();
            }

            Role role = await _roleService.GetRoleById(roleId: user.RoleId.ToString());

            // 2. Verificăm Rolul
            if (role == null || role.Name != "Admin")
            {
                ViewBag.Error = "Nu aveți permisiunea de a accesa această zonă.";
                return View();
            }

            // 3. Creăm Claims pentru Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false // Ține minte utilizatorul
            };

            // 4. Scriem Cookie-ul (Login efectiv)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
