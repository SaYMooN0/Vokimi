using Vokimi.Models.Static;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vokimi.Models.DataBaseClasses;
using VokimiServices;
using Microsoft.AspNetCore.Authentication;
using Vokimi.Models.ViewModels.Account;

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
        [HttpGet]
        public IActionResult Authorization()
        {
            if (HttpContext.GetUserIdFromIdentity() != -1)
                return RedirectToAction("MyAccount");
            return View(new AuthorizationViewModel());
        }
        [HttpPost]
        async public Task<IActionResult> Authorization(AuthorizationViewModel model)
        {
            if (!await _dataBase.AnyUserWithSuchEmail(model.Email))
            {
                model.ErrorMessage = "Unregistered email";
                return View(model);
            }

            User? user=await _dataBase.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
            if(user==null)
            {
                model.ErrorMessage = "Invalid password";
                return View(model);
            }
            await TrySetAuthorizationCookiesAsync(user);
            return RedirectToAction("Index","Tests");
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
                int id = await _dataBase.AddNewUser(user);
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
            int userId = HttpContext.GetUserIdFromIdentity();
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
        [HttpPost]
        async public Task<IActionResult> LogOut()
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return Unauthorized(new { message = "Unauthorized" });
            await HttpContext.SignOutAsync(); 
            return Ok(new { message = "Logged out successfully" });
        }
        private async Task<bool> TrySetAuthorizationCookiesAsync(string email, string password)
        {
            User? user = await _dataBase.GetUserByEmailAndPasswordAsync(email, password);
            if (user == null)
                return false;
            return await TrySetAuthorizationCookiesAsync(user);
        }
        private async Task<bool> TrySetAuthorizationCookiesAsync(User user)
        {
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
    }
}
