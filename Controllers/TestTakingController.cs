using Microsoft.AspNetCore.Mvc;

namespace Vokimi.Controllers
{
    public class TestTakingController : Controller
    {
        public IActionResult Test(int testId)
        {
            return View();
        }
    }
}
