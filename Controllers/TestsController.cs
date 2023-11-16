using Microsoft.AspNetCore.Mvc;
using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.Static;
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
        public async Task<IActionResult> Index()
        {
            CatalogViewModel vm = new();
            vm.Tests = (await _dataBase.GetAllTestsMainInfoAsync(HttpContext.GetUserIdFromIdentity())).ToList();
            //vm.FilterTests(); 
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CatalogViewModel vm)
        {
            vm.Tests = (await _dataBase.GetAllTestsMainInfoAsync(HttpContext.GetUserIdFromIdentity())).ToList();
            //vm.FilterTests(); 
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string tag)
        {
            CatalogViewModel vm = new();
            vm.Tests = (await _dataBase.GetAllTestsMainInfoAsync(HttpContext.GetUserIdFromIdentity())).ToList();
            vm.Filter.ChosenTags = new() { tag };
            //vm.FilterTests(); 
            return View(vm);
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

            string author = await _dataBase.GetUserNameById(t.AuthorId);
            TestViewModel vm = new TestViewModel(t, author);
            vm.Comments = (await _dataBase.GetCommentsInfoForTest(t.Id)).ToList();
            return View(vm);
        }
        async public Task<IActionResult> NewComment(int testId, string commentText)
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return RedirectToAction("Authorization", "Account");
            await _dataBase.AddCommentForTest(testId, commentText, userId);
            return RedirectToAction("Test", new { id = testId });
        }
        [HttpPost]
        public async Task<IActionResult> RateTest(int testId, int rating)
        {
            return Ok(new { CurrentUserRating = rating });
        }


    }
}
