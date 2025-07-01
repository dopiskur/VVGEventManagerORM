using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;
using eventLib.Models;
using eventLib.Security;
using eventLib.Dal;

namespace exercise_13.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            var loginVm = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVm)
        {
            // Try to get a user from database
            User userLogin = _repository.UserGet(null, loginVm.Username);

            if (userLogin == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            // Check is password hash matches
            var b64hash = AuthenticationProvider.GetHash(loginVm.Password, userLogin.PwdSalt);
            if (b64hash != userLogin.PwdHash)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, userLogin.Username),
                new Claim(ClaimTypes.Role, userLogin.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            // We need to wrap async code here into synchronous since we don't use async methods
            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (loginVm.ReturnUrl != null)
                return LocalRedirect(loginVm.ReturnUrl);
            else if (userLogin.RoleName == "Admin")
                return RedirectToAction("Index", "EventManagement");
            else if (userLogin.RoleName == "User")
                return RedirectToAction("Index", "Events");
            else
                return View();
        }

        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserVM userVm)
        {
            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = userVm.Username.Trim();
                if (_repository.UserGet(null, userVm.Username) != null)
                    return BadRequest($"Username {trimmedUsername} already exists");

                // Hash the password
                var b64salt = AuthenticationProvider.GetSalt();
                var b64hash = AuthenticationProvider.GetHash(userVm.Password, b64salt);

                // Create user from DTO and hashed password
                var user = new User
                {
                    Username = userVm.Username,
                    PwdHash = b64hash,
                    PwdSalt = b64salt,
                    FirstName = userVm.FirstName,
                    LastName = userVm.LastName,
                    Email = userVm.Email,
                    Phone = userVm.Phone,
                    UserRoleId = 2, // regular user
                };

                // Add user and save changes to database
                userVm.IDUser = _repository.UserAdd(user);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
