using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.Static;
using Vokimi.Models.ViewModels.TestTaking;
using VokimiServices;

namespace Vokimi.Controllers
{
    public class TestTakingController : Controller
    {
        private IDataBase _dataBase;
        private VokimiServices.ILogger _logger;

        public TestTakingController(IDataBase dataBase, VokimiServices.ILogger logger)
        {
            _dataBase = dataBase;
            _logger = logger;
        }
        public async Task<IActionResult> Test(int id)
        {
            TestTakingViewModel vm = new();
            Test t = await _dataBase.GetTestByIdAsync(id);
            vm.TestId = t.Id;
            vm.Questions = (await _dataBase.GetQuestionsForTestAsync(id)).ToList();
            return View(vm);
        }
        [HttpPost]
        public IActionResult TestTaken(List<int> answers, int testId)
        {
            int sum = 0;
            foreach (int item in answers)
                sum += item;
            int userId = HttpContext.GetUserIdFromIdentity();

            if (userId == -1) userId = 1; //for unauthorized users

            _dataBase.AddNewTestTaking(userId, testId, sum, DateTime.Now);
            return RedirectToAction("Result", new ResultData(testId, sum));

        }
        [HttpGet]
        public async Task<ActionResult> Result(ResultData data)
        {
            Test? t = await _dataBase.GetTestByIdAsync(data.TestId);
            if (t is null) RedirectToAction("Error");

            Result? result = t.Results.FirstOrDefault(res => res.GapMin <= data.Points && res.GapMax >= data.Points);

            if (result is null) RedirectToAction("Error");
            ResultViewModel vm = new(t.Id, t.Name,result, t.Results);
            return View(vm);
        }

        public record ResultData(int TestId, int Points);

    }
}
