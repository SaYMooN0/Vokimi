using Microsoft.AspNetCore.Mvc;
using Vokimi.Models.DataBaseClasses;
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
            Test t= await _dataBase.GetTestByIdAsync(id);
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
            return RedirectToAction("Result", new Tuple<int, int>(sum, testId));
        }
        [HttpGet]
        public IActionResult Result(Tuple<int, int> tuple)
        {
            return View(tuple);
        }

    }
}
