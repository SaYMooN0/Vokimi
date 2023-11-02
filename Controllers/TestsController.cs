using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.Static;

namespace Vokimi.Controllers
{
    public class TestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
