using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.Static;
using Vokimi.Models.ViewModels;

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
            var a = TestCreationQuestionsViewModel.FromQuestions(QuestionsInSession);
            return View(a);
        }
        [HttpPost]
        public IActionResult Questions(TestCreationQuestionsViewModel questionsDataViewModel)
        {
            List<Question> _newQuestions = new List<Question>();
            questionsDataViewModel.ErrorMessage = String.Empty;
            foreach (QuestionData question in questionsDataViewModel.Questions)
            {
                if (string.IsNullOrEmpty(question.Text) || question.Text.Length < 5 || question.Text.Length > 250)
                {
                    questionsDataViewModel.ErrorMessage =
                        "Error: Failed to save questions. The length of question cannot be less than 5 or more than 250 characters. " +
                        $"The length of the question number {questionsDataViewModel.Questions.IndexOf(question) + 1} is {question.Text?.Length ?? 0} characters";
                    return View(questionsDataViewModel);
                }
                if (question.AnswerOptions.Count < 2 || question.AnswerOptions.Count > 15)
                {
                    questionsDataViewModel.ErrorMessage =
                        "Error: Failed to save questions. The number of answers to one question cannot be less than 2 or more than 15. " +
                        $"Question number {questionsDataViewModel.Questions.IndexOf(question) + 1} has {question.AnswerOptions.Count} answer options";
                    return View(questionsDataViewModel);
                }
                foreach (var answerOption in question.AnswerOptions)
                {
                    if (string.IsNullOrEmpty(answerOption.Key) || answerOption.Key.Length < 1 || answerOption.Key.Length > 220)
                    {
                        questionsDataViewModel.ErrorMessage =
                            "Error: Failed to save questions. The length of answer cannot be less than 1 or more than 220 characters. " +
                            $"The length of one of the answer for the question number {questionsDataViewModel.Questions.IndexOf(question) + 1} is {answerOption.Key?.Length ?? 0} characters";
                        return View(questionsDataViewModel);
                    }
                    if (answerOption.Value > 90 || answerOption.Value < -90)
                    {
                        questionsDataViewModel.ErrorMessage =
                            "Error: Failed to save questions. The number of points that can be obtained for the answer can not be less than -90 and more than 90";
                        return View(questionsDataViewModel);
                    }
                }
                _newQuestions.Add(new(question.Text, question.AnswerOptions));
            }
            QuestionsInSession = _newQuestions;
            return View(questionsDataViewModel);
        }
        [HttpGet]
        public IActionResult Results()
        {
            int minPoints= CalculateMinimumPointsForQuestions(),
                maxPoints= CalculateMaximumPointsForQuestions();

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
            return View(res);
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
            {
                return TestCreationData?.Questions ?? new List<Question>();
            }
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


    }

}
