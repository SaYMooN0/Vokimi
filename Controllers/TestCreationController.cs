using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.Static;

namespace Vokimi.Controllers
{
    public class TestCreationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            int id = HttpContext.GetUserIdFromIdentity();
            if (id == -1)
                return RedirectToAction("Authorization", "Account");
            return View(TestCreationData);
        }
        [HttpPost]
        public IActionResult SaveTestCreationInfo(TestCreationData newData)
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return Unauthorized(new { message = "Make sure you are logged in to your account" });

            if (TestCreationData == null) TestCreationData = new();

            TestCreationData.TestName = newData.TestName ?? TestCreationData.TestName;
            TestCreationData.Description = newData.Description ?? TestCreationData.Description;
            TestCreationData.AgeRestriction = newData.AgeRestriction;
            TestCreationData.Language = newData.Language;

            return Ok(new { message = "Success" });
        }
        [HttpPost]
        public IActionResult Questions() { return View(QuestionsInSession); }
        [HttpPost]
        public IActionResult SaveQuestions(List<Question> questions)
        {
            QuestionsInSession = questions;
            return Ok(new { message = "Success" });
        }

        [HttpPost]
        public IActionResult Results() { return View(ResultsInSession); }

        [HttpPost]
        public IActionResult SaveResults(List<Result> results)
        {
            ResultsInSession = results;
            return Ok(new { message = "Success" });
        }
        private TestCreationData TestCreationData
        {
            get => HttpContext.Session.GetString("_viewModel") is string jsonData
                    ? JsonConvert.DeserializeObject<TestCreationData>(jsonData)
                    : new();
            set => HttpContext.Session.SetString("_viewModel", JsonConvert.SerializeObject(value));
        }
        private List<Question> QuestionsInSession
        {
            get { return TestCreationData?.Questions ?? new List<Question>(); }
            set
            {
                TestCreationData ??= new TestCreationData();
                TestCreationData.Questions = value;
            }
        }
        private List<Result> ResultsInSession
        {
            get { return TestCreationData?.Results ?? new List<Result>(); }
            set
            {
                TestCreationData ??= new TestCreationData();
                TestCreationData.Results = value;
            }
        }

    }
}
