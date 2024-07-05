using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestResult
    {
        public DraftTestResultId Id { get; init; }
        public DraftTestId TestId { get; init; }
        public string StringId { get; init; }
        public string Text { get; private set; }
        public string? ImagePath { get; private set; }
        public static DraftTestResult CreateNew(string stringId, DraftTestId testId) => new() {
            Id = new(),
            TestId = testId,
            StringId = stringId,
            Text = string.Empty,
        };

        public virtual ICollection<BaseAnswer> AnswersLeadingToResult { get; set; } = [];

    }
}
