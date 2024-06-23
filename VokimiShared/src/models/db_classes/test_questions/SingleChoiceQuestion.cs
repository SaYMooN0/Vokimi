using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_questions
{
    public class SingleChoiceQuestion<T> : BaseQuestion<T> where T : BaseAnswer
    {
        public override int MaxPossiblePoints() {
            throw new NotImplementedException();
        }

        public override int MinPossiblePoints() {
            throw new NotImplementedException();
        }
    }
}
