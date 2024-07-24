using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test.test_questions;

namespace VokimiShared.src.models.db_classes.test
{
    public class GenericTestQuestion
    {
        public QuestionId Id { get; init; }
        public string Text { get; init; }
        public string? ImagePath { get; init; }
        public bool ShuffleAnswers { get; init; }
        public AnswersType AnswersType { get; init; }

        //if null question is not multi choice
        public MultiChoiceQuestionDataId? MultiChoiceQuestionDataId { get; init; }
        public virtual MultiChoiceQuestionData? MultiChoiceQuestionData { get; private set; }
        

        //answers
        //public virtual ICollection<BaseAnswer> Answers { get; private set; } = new List<BaseAnswer>();
    }
}
