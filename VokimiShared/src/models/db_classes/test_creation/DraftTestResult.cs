using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestResult
    {
        public DraftTestResultId Id { get; init; }
        public DraftTestId TestId { get; init; }
        public string StringId { get; init; }
        public string? Text { get; private set; }
        public string? ImagePath { get; private set; }
        public virtual ICollection<DraftTestAnswer> AnswersLeadingToResult { get; set; } = [];

        public static DraftTestResult CreateNew(string stringId, DraftTestId testId) =>
            CreateNew(stringId, testId, string.Empty, null);
        public static DraftTestResult CreateNew(
            string stringId, DraftTestId testId, string? text, string? imagePath) =>
            new() {
                Id = new(),
                TestId = testId,
                StringId = stringId,
                Text = text,
                ImagePath = imagePath
            };
        public void Update(string text, string? imagePath) {
            Text = text;
            ImagePath = imagePath;
        }

    }
}
