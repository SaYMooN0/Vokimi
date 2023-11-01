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
        public IActionResult Index(TestCreationData newData)
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return Unauthorized(new { message = "Make sure you are logged in to your account" });

            var currentData = TestCreationData;

            currentData.TestName = newData.TestName;
            currentData.Description = newData.Description;
            currentData.AgeRestriction = newData.AgeRestriction;
            currentData.Language = newData.Language;

            TestCreationData = currentData;

            return View(TestCreationData);
        }
        [HttpGet]
        public IActionResult Questions()
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return RedirectToAction("Authorization", "Account");
            return View(QuestionsInSession);
        }
        [HttpPost("QuestionsSave")]
        //public IActionResult Questions(List<Question> questions)
        //{
        //    QuestionsInSession = questions;
        //    return Ok(new { message = "Success" });
        //}
        public IActionResult Questions(List<object> value)
        {
           
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
