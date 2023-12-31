﻿using Microsoft.AspNetCore.Mvc;
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
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CatalogViewModel vm)
        {
            vm.Tests = (await _dataBase.GetAllTestsMainInfoAsync(HttpContext.GetUserIdFromIdentity())).ToList();
            vm.FilterTests();
            if (vm.Filter.OnlyPinned)
            {
                int userId = HttpContext.GetUserIdFromIdentity();
                if (userId == -1)
                {
                    vm.TopMessage = "The only pinned field was not considered in filtering. Log in to your account";
                    return View(vm);
                }
                IEnumerable<int> pinnedTests = await _dataBase.GetPinnedTestsForUser(userId);
                vm.FilterByInEnumerable(pinnedTests);
            }
            return View(vm);
        }
        [HttpPost]
        [Route("Index/tag={tag}")]
        public async Task<IActionResult> Index(string tag)
        {
            CatalogViewModel vm = new();
            vm.Tests = (await _dataBase.GetAllTestsMainInfoAsync(HttpContext.GetUserIdFromIdentity())).ToList();
            vm.Filter.ChosenTags = new() { tag };
            vm.FilterByTags();
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
            vm.IsPinned = await _dataBase.IsTestPinnedByUserAsync((int)id, t.AuthorId);
            
            
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1) return View(vm);

            TestsRating? testsRating = t.Ratings.FirstOrDefault(r => r.UserId == userId);
            vm.CurrentUserRating = testsRating!=null? testsRating.Rating : null;
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
        public async Task<IActionResult> RateTest([FromBody] TestRatingData testRating)
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return RedirectToAction("Authorization", "Account");
            await _dataBase.RateTestAsync(testRating.TestId, testRating.Rating, userId);
            return Ok(new { CurrentUserRating = testRating.Rating });
        }

        [HttpPost]
        public async Task<IActionResult> PinUnpinTest([FromBody] int testId)
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return RedirectToAction("Authorization", "Account");
            bool wasPinned = await _dataBase.PinUnpinTestForUser(testId, userId);
            return Ok(new { wasPinned });
        }

    }
    public class TestRatingData
    {
        public int TestId { get; set; }
        public short Rating { get; set; }
    }
}
