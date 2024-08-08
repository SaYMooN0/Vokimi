using VokimiShared.src.enums;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_creation.generic_test_related
{
    public class DraftGenericTestQuestion
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; private set; }
        public string? ImagePath { get; private set; }
        public bool ShuffleAnswers { get; private set; }
        public AnswersType AnswersType { get; init; }
        public ushort OrderInTest { get; private set; }
        public bool IsMultipleChoice { get; private set; }
        public MultipleChoiceAdditionalData? MultipleChoiceData { get; private set; }
        public virtual ICollection<DraftGenericTestAnswer> Answers { get; private set; } = new List<DraftGenericTestAnswer>();

        public DraftTestId DraftTestId { get; init; }
        public static DraftGenericTestQuestion CreateNew(string text, AnswersType answersType, DraftTestId testId, ushort orderInTest) => new() {
            Id = new(),
            Text = text,
            ImagePath = null,
            ShuffleAnswers = false,
            AnswersType = answersType,
            OrderInTest = orderInTest,
            IsMultipleChoice = false,
            MultipleChoiceData = null,
            DraftTestId = testId,
            Answers = new List<DraftGenericTestAnswer>()
        };
        public void UpdateOrderInTest(ushort newOrder) => OrderInTest = newOrder;
        public void UpdateAsSingleChoice(
            string text,
            string? imagePath,
            bool shuffleAnswers,
            ICollection<DraftGenericTestAnswer> answers) {

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
            ICollection<DraftGenericTestAnswer> answers,
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
