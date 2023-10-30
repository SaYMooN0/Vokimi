using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels;
using VokimiServices;

namespace Vokimi.Controllers
{
    public class AccountController : Controller
    {
        private IDataBase _dataBase;
        private VokimiServices.ILogger _logger;

        public AccountController(IDataBase dataBase, VokimiServices.ILogger logger)
        {
            _dataBase = dataBase;
            _logger = logger;
        }

        public IActionResult Authorization()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View(new RegistrationViewModel());
        }

        [HttpPost]
        async public Task<IActionResult> Registration(RegistrationViewModel model)
        {
            _logger.Runtime($"User with params {model} trying to register");
            if (model.IsValidEmail())
            {
                _logger.Runtime($"Checking if any user with email {{{model.Email}}}");
                if (await _dataBase.AnyUserWithSuchEmail(model.Email))
                {
                    model.Email = "This email is already taken";
                    return View(model);
                }
            }
            if (model.IsValid())
            {
                User user = new User(model.Nickname, model.Email, model.Password, model.BirthDate);
                int id = await _dataBase.AddUser(user);
                _logger.Runtime($"User with params {model} and id={id} has registered successfully");
                _logger.Info($"New user with params {model} and id={id} registered");
                return RedirectToAction("SuccessfulRegistration");
            }
            return View(model);
        }
        public IActionResult SuccessfulRegistration()
        {
            return View();
        }
        async public Task<IActionResult> MyAccount()
        {
            int userId = GetUserIdFromIdentity();
            if (userId == -1) return RedirectToAction("Authorization");
            MyAccountViewModel? viewModel = await _dataBase.GetMyAccountInfo(userId);
            return View(viewModel);
        }
        public IActionResult UserNotFound()
        {
            return View();
        }
        async public Task<IActionResult> UserProfile(int? id)
        {
            if (id is null || id < 0)
                return RedirectToAction("UserNotFound");
            UserProfileViewModel? viewModel = await _dataBase.GetUserInfo((int)id);
            if (viewModel == null)
                return RedirectToAction("UserNotFound");
            return View(viewModel);
        }
        private async Task<bool> TrySetAuthorithationCookiesAsync(string email, string password)
        {
            User? user = await _dataBase.GetUserByEmailAndPasswordAsync(email, password);
            if (user == null)
                return false;
            ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                new Claim("email", user.Email),
                new Claim("password", user.Password),
                new Claim("isBanned", user.IsBanned.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("userId", user.Id.ToString()),
            }, "ApplicationCookie", ClaimTypes.Name, ClaimTypes.Role);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            return true;
        }
        private int GetUserIdFromIdentity()
        {
            var identity = HttpContext.User.Identities.FirstOrDefault(i => i.Claims.Any(c => c.Type == "userId"));
            if (identity != null)
            {
                var claim = identity.Claims.FirstOrDefault(c => c.Type == "userId");
                if (claim != null && Int32.TryParse(claim.Value, out int userId))
                    return userId;
            }
            return -1;
        }
    }
}
