using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_results.results_for_draft_tests
{
    public class DraftTestResult
    {
        public DraftTestResultId Id { get; init; }
        public DraftTestId TestId { get; init; }
        public string StringId { get; init; }
        public string? Text { get; private set; }
        public string? ImagePath { get; private set; }
        public DraftTestTypeSpecificResultDataId TestTypeSpecificDataId { get; private set; }
        public virtual DraftTestTypeSpecificResultData TestTypeSpecificData { get; private set; }
        public static DraftTestResult CreateNew(
            string stringId,
            DraftTestId testId,
            DraftTestTypeSpecificResultDataId testTypeSpecificDataId)
            => CreateNew(stringId, testId, string.Empty, string.Empty, testTypeSpecificDataId);
        public static DraftTestResult CreateNew(
            string stringId,
            DraftTestId testId,
            string? text,
            string? imagePath,
            DraftTestTypeSpecificResultDataId testTypeSpecificDataId)
            => new() {
                Id = new(),
                TestId = testId,
                StringId = stringId,
                Text = text,
                ImagePath = imagePath,
                TestTypeSpecificDataId= testTypeSpecificDataId
            };
        public void Update(string text, string? imagePath) {
            Text = text;
            ImagePath = imagePath;
        }
    }
}
