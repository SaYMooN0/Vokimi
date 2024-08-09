using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_results.results_for_draft_tests
{
    public class DraftTestResult
    {
        public DraftTestResultId Id { get; init; }
        public DraftTestId TestId { get; init; }
        public string Name { get; private set; }
        public string? Text { get; private set; }
        public string? ImagePath { get; private set; }
        public DraftTestTypeSpecificResultDataId TestTypeSpecificDataId { get; private set; }
        public virtual DraftTestTypeSpecificResultData TestTypeSpecificData { get; private set; }
        public static DraftTestResult CreateNew(
            string name, DraftTestId testId, DraftTestTypeSpecificResultDataId testTypeSpecificDataId)
            => CreateNew(testId, name, string.Empty, string.Empty, testTypeSpecificDataId);
        public static DraftTestResult CreateNew(
            DraftTestId testId,
            string name,
            string? text,
            string? imagePath,
            DraftTestTypeSpecificResultDataId testTypeSpecificDataId)
            => new() {
                Id = new(),
                TestId = testId,
                Name = name,
                Text = text,
                ImagePath = imagePath,
                TestTypeSpecificDataId = testTypeSpecificDataId
            };
        public void Update(string name, string text, string? imagePath) {
            Text = text;
            ImagePath = imagePath;
        }
    }
}
