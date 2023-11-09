using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels.Tests;
using VokimiServices;

namespace Vokimi.Controllers
{
    public class TestsController : Controller
    {
        private IDataBase _dataBase;
        private VokimiServices.ILogger _logger;

        public TestsController(IDataBase dataBase, VokimiServices.ILogger logger)
        {
            _dataBase = dataBase;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TestNotFound()
        {
            return View();
        }
        async public Task<IActionResult> Test(int? id)
        {
            if (id is null)
                return RedirectToAction("Index");
            Test? t = await _dataBase.GetTestByIdAsync((int)id);
            if (t is null)
                return RedirectToAction("TestNotFound");
            TestViewModel vm=new TestViewModel()
            {
                Id=t.Id,
                TestName=t.Name,
                AuthorId=t.AuthorId,
            
            };
            return View(vm);
        }
    }
}
