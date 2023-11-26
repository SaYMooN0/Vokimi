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
        public async Task<IActionResult> TestTaken(List<int> answers, int testId)
        {
            int sum = 0;
            foreach (int item in answers)
                sum += item;

            int? resultId = await _dataBase.GetResultIdByTestAndPoints(testId, sum);
            if (resultId is null) RedirectToAction("Error");

            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1) userId = 1; //for unauthorized users

            await _dataBase.AddTestTakingAsync(userId, testId, (int)resultId, DateTime.Now);
            return RedirectToAction("Result", new ResultData(testId, (int)resultId));

        }
        [HttpGet]
        public async Task<ActionResult> Result(ResultData data)
        {
            Test? t = await _dataBase.GetTestByIdAsync(data.TestId);
            if (t is null) RedirectToAction("Error");

            Result? result = t.Results.FirstOrDefault(res => res.Id==data.resultId);

            if (result is null) RedirectToAction("Error");
            ResultViewModel vm = new(t.Id, t.Name, result, t.Results);
            vm.ResultsFrequency = await _dataBase.GetResultsIdWithFrequencyForTest(t.Id);
            vm.TestTakingsCount = vm.ResultsFrequency.Values.Sum();
            foreach (var res in vm.ResultsFrequency)
            {
                double p= (double)res.Value / (double)vm.TestTakingsCount;
                double p1 = Math.Round(p * 100);
                vm.ResultsFrequency[res.Key] = (int)p1;
            }
            return View(vm);
        }

        public record ResultData(int TestId, int resultId);

    }
}
