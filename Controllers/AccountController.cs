using Microsoft.AspNetCore.Mvc;
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
                User user = new User(model.Nickname,model.Email,model.Password, model.BirthDate);
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
        public IActionResult MyAccount()
        {
            return View();
        }
        public IActionResult UserNotFound()
        {
            return View();
        }
        async public Task<IActionResult> UserProfile(int? id)
        {
            if (id is null || id<0)
                return RedirectToAction("UserNotFound");
            UserProfileViewModel? viewModel = await _dataBase.GetUserInfo((int)id);
            if (viewModel == null)
                return RedirectToAction("UserNotFound");
            return View(viewModel);
        }
    }
}
