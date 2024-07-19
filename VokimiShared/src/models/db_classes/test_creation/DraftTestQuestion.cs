using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestQuestion
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; private set; }
        public string? ImagePath { get; private set; }
        public bool ShuffleAnswers { get; private set; }
        public AnswersType AnswersType { get; init; }
        public bool IsMultipleChoice { get; private set; }
        public MultipleChoiceAdditionalData? MultipleChoiceData { get; private set; }
        public virtual ICollection<DraftTestAnswer> Answers { get; private set; } = new List<DraftTestAnswer>();

        public DraftTestId DraftTestId { get; init; }
        public static DraftTestQuestion CreateNew(string text, AnswersType answersType, DraftTestId testId) => new() {
            Id = new(),
            Text = text,
            ImagePath = null,
            ShuffleAnswers = false,
            AnswersType = answersType,
            IsMultipleChoice = false,
            MultipleChoiceData = null,
            DraftTestId = testId,
            Answers = new List<DraftTestAnswer>()
        };
        public void UpdateAsSingleChoice(
            string text,
            string? imagePath,
            bool shuffleAnswers,
            ICollection<DraftTestAnswer> answers) {

            Text = text;
            ImagePath = imagePath;
            ShuffleAnswers = shuffleAnswers;
            Answers = answers;

            IsMultipleChoice = false;
            MultipleChoiceData = null;

        }
        public void UpdateAsMultipleChoice(
            string text,
            string? imagePath,
            bool shuffleAnswers,
            ICollection<DraftTestAnswer> answers,
            MultipleChoiceAdditionalData multipleChoiceData) {

            Text = text;
            ImagePath = imagePath;
            ShuffleAnswers = shuffleAnswers;
            Answers = answers;

            IsMultipleChoice = true;
            MultipleChoiceData = multipleChoiceData;
        }

    }

    public class MultipleChoiceAdditionalData
    {
        public ushort MinAnswers { get; init; }
        public ushort MaxAnswers { get; init; }
    }

}
