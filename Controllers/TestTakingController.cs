using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.ViewModels.TestTaking;

namespace Vokimi.Controllers
{
    public class TestTakingController : Controller
    {
        public IActionResult Test(int id)
        {
            TestTakingViewModel vm = new();
            return View(vm);
        }
        [HttpPost]
        public IActionResult TestTaken()
        {
            TestTakingViewModel vm = new();
            return Ok();
        }
    }
}
