using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_questions
{
    public class SingleChoice<T> : IQuestion<T> where T : BaseAnswer
    {
        public QuestionId Id { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string Text { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? ImagePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<T> Answers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int MaxPossiblePoints() {
            throw new NotImplementedException();
        }

        public int MinPossiblePoints() {
            throw new NotImplementedException();
        }
    }
}
