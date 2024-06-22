using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_questions
{
    public interface IQuestion<T> where T : BaseAnswer
    {
        public QuestionId Id { get; init; }
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public List<T> Answers { get; set; }
        public int MinPossiblePoints();
        public int MaxPossiblePoints();
    }
}
