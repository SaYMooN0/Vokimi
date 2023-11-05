using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels
{
    public class TestCreationQuestionsViewModel
    {
        public List<QuestionData> Questions { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public static TestCreationQuestionsViewModel FromQuestions(List<Question> questions)
        {
            TestCreationQuestionsViewModel _vm = new();
            foreach (Question q in questions)
            {
                if (q != null)
                    _vm.Questions.Add(new QuestionData { Text = q.Text, AnswerOptions = q.AnswerOptions });
            }
            return _vm;
        }
    }
    public class QuestionData
    {
        public string Text { get; set; }
        public Dictionary<string, int> AnswerOptions { get; set; }
    }
}
