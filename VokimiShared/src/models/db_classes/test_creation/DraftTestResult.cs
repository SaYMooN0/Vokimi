namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestResult
    {
        public DraftTestResultId Id { get; init; }
        public string StringId { get; init; }
        public string Text { get; private set; }
        public string? ImagePath { get; private set; }
        public static DraftTestResult CreateNew(string stringId) => new() {
            Id = new(),
            StringId = stringId,
            Text = string.Empty,
        };
    }
}
