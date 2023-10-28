using Microsoft.AspNetCore.Mvc;

namespace Vokimi.Controllers
{
    public class TestsController : Controller
    {
        public IActionResult Catalog()
        {
            return View();
        }
        public IActionResult NewTest()
        {
            return View();
        }
    }
}
