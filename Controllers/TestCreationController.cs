using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.Static;
using Vokimi.Models.ViewModels;
using VokimiServices;

namespace Vokimi.Controllers
{
    public class TestCreationController : Controller
    {
        private IDataBase _dataBase;
        private VokimiServices.ILogger _logger;
        public TestCreationController(IDataBase dataBase, VokimiServices.ILogger logger)
        {
            _dataBase = dataBase;
            _logger = logger;
        }
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
            var a = TestCreationQuestionsViewModel.FromQuestions(QuestionsInSession);
            return View(a);
        }
        [HttpPost]
        public IActionResult Questions(TestCreationQuestionsViewModel questionsDataViewModel)
        {
            questionsDataViewModel.ErrorMessage = QuestionsValidationString(questionsDataViewModel);

            if (!string.IsNullOrEmpty(questionsDataViewModel.ErrorMessage))
                return View(questionsDataViewModel);

            List<Question> _newQuestions = questionsDataViewModel.Questions
                .Select(q => new Question(q.Text, q.AnswerOptions))
                .ToList();

            QuestionsInSession = _newQuestions;

            return View(questionsDataViewModel);
        }
        [HttpGet]
        public IActionResult Results()
        {
            int minPoints = CalculateMinimumPointsForQuestions(),
                maxPoints = CalculateMaximumPointsForQuestions();

            var vm = new TestCreationResultsViewModel(
                maxPoints,
                minPoints,
                ResultsInSession);

            if (minPoints == maxPoints)
                vm.ErrorMessage = "Please create questions first";

            return View(vm);
        }

        [HttpPost]
        public IActionResult Results(TestCreationResultsViewModel? res)
        {
            if (res == null)
                return Results();

            res.ErrorMessage = ResultsValidationString(res.ResultItems);

            if (!string.IsNullOrEmpty(res.ErrorMessage))
                return View(res);
            List<Result> _newResults = new();
            foreach (var item in res.ResultItems)
            {
                _newResults.Add(new(item.MainText, item.Description, item.From, item.To));
            }
            ResultsInSession = _newResults;
            return View(res);
        }
        [HttpPost]
        public IActionResult FinishCreation()
        {
            int userId = HttpContext.GetUserIdFromIdentity();
            if (userId == -1)
                return RedirectToAction("Authorization", "Account");

            if (string.IsNullOrEmpty(TestCreationData.TestName))
                return BadRequest("Please fill in the main information about the test.");
            if(QuestionsInSession is null || QuestionsInSession.Count<1)
                return BadRequest("Please create questions for the test.");
            if (ResultsInSession is null || ResultsInSession.Count < 1)
                return BadRequest("Please create results for the test.");

            //_dataBase.AddNewTest();
            return Ok();
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
            get
            { return TestCreationData?.Questions ?? new List<Question>(); }
            set
            {
                TestCreationData newData = TestCreationData ?? new();
                newData.Questions = value;
                TestCreationData = newData;
            }
        }
        private List<Result> ResultsInSession
        {
            get { return TestCreationData?.Results ?? new List<Result>(); }
            set
            {
                TestCreationData newData = TestCreationData ?? new();
                newData.Results = value;
                TestCreationData = newData;
            }
        }
        private int CalculateMinimumPointsForQuestions()
        {
            int minPoints = 0;
            foreach (var question in QuestionsInSession)
            {
                minPoints += question.AnswerOptions.Values.Min();
            }
            return minPoints;
        }
        private int CalculateMaximumPointsForQuestions()
        {
            int maxPoints = 0;
            foreach (var question in QuestionsInSession)
            {
                maxPoints += question.AnswerOptions.Values.Max();
            }
            return maxPoints;
        }
        private string ResultsValidationString(List<ResultData> resultItems)
        {
            if (resultItems.Count < 2)
                return "There cannot be less than 2 results for the test.";
            if (resultItems == null || !resultItems.Any())
                return "No results to validate.";
            int minPoints = CalculateMinimumPointsForQuestions(),
               maxPoints = CalculateMaximumPointsForQuestions();
            if (resultItems.Any(r => r.From < minPoints))
                return "The value of \"From\" cannot be less than the minimum number of points that can be obtained for answering questions.";

            if (resultItems.Any(r => r.To > maxPoints))
                return "The value of \"To\" cannot be greater than the maximum number of points that can be obtained for answering questions.";

            foreach (var result in resultItems)
            {
                if (result.From >= result.To)
                    return "The value of \"From\" must be less than the value of \"To\" for each result.";
            }

            var sortedResults = resultItems.OrderBy(r => r.From).ToList();
            int previousTo = minPoints - 1;

            foreach (var result in sortedResults)
            {
                if (result.From <= previousTo)
                    return "The ranges of results cannot overlap.";
                if (result.From > previousTo + 1)
                    return "There should be no gaps between the ranges of results.";
                previousTo = result.To;
            }

            if (previousTo < maxPoints)
                return "There should be no gaps between the last range and the maximum points possible.";

            return string.Empty;
        }
        private string QuestionsValidationString(TestCreationQuestionsViewModel questionsDataViewModel)
        {
            foreach (QuestionData question in questionsDataViewModel.Questions)
            {
                if (string.IsNullOrEmpty(question.Text) || question.Text.Length < 5 || question.Text.Length > 250)
                    return $"Error: Failed to save questions. The length of question cannot be less than 5 or more than 250 characters. " +
                           $"The length of the question number {questionsDataViewModel.Questions.IndexOf(question) + 1} is {question.Text?.Length ?? 0} characters.";
                
                if (question.AnswerOptions.Count < 2 || question.AnswerOptions.Count > 15)
                    return $"Error: Failed to save questions. The number of answers to one question cannot be less than 2 or more than 15. " +
                           $"Question number {questionsDataViewModel.Questions.IndexOf(question) + 1} has {question.AnswerOptions.Count} answer options.";
                
                foreach (var answerOption in question.AnswerOptions)
                {
                    if (string.IsNullOrEmpty(answerOption.Key) || answerOption.Key.Length < 1 || answerOption.Key.Length > 220)
                        return $"Error: Failed to save questions. The length of answer cannot be less than 1 or more than 220 characters. " +
                               $"The length of one of the answer for the question number {questionsDataViewModel.Questions.IndexOf(question) + 1} is {answerOption.Key?.Length ?? 0} characters.";
                    
                    if (answerOption.Value > 90 || answerOption.Value < -90)
                        return "Error: Failed to save questions. The number of points that can be obtained for the answer cannot be less than -90 and more than 90.";
                    
                }
            }
            return string.Empty;
        }
    }

}
