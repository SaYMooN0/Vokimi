using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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
            if (!model.IsValid())
            {
                _logger.Runtime($"User with params {model} trying to register");
                if (await _dataBase.AnyUserWithSuchEmail(model.Email))
                {
                    return View(model);
                }
                return View(model);
            }
            return View(model);
        }
    }
}
