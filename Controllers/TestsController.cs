using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.Static;

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
            int id = HttpContext.GetUserIdFromIdentity();
            if (id == -1)
                return RedirectToAction("Authorization","Account");
            return View();
        }
    }
}
