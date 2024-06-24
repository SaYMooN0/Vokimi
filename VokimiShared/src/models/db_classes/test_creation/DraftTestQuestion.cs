using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestQuestion
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; init; }
        public string? ImagePath { get; init; }
        public bool ShuffleAnswers { get; init; }
        public AnswersType AnswersType { get; init; }
        public bool IsMultipleChoice { get; init; }
        public MultipleChoiceAdditionalData? MultipleChoiceData { get; init; }

        public DraftTestId DraftTestId { get; init; }
        public virtual ICollection<BaseAnswer> Answers { get; private set; } = new List<BaseAnswer>();

        public int MinPossiblePoints() => throw new NotImplementedException();
        public int MaxPossiblePoints() => throw new NotImplementedException();
        public static DraftTestQuestion CreateNew(string text, AnswersType answersType, DraftTestId testId) => new() {
            Id = new(),
            Text = text,
            ImagePath = null,
            ShuffleAnswers = false,
            AnswersType = answersType,
            IsMultipleChoice = false,
            MultipleChoiceData = null,
            DraftTestId = testId,
            Answers = new List<BaseAnswer>()
        };

    }

    public class MultipleChoiceAdditionalData
    {
        public int MinAnswers { get; init; }
        public int MaxAnswers { get; init; }
        public bool UseAverageScore { get; init; }
    }

}
