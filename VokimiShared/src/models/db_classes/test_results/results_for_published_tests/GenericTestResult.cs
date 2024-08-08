using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_results.results_for_published_tests
{
    public class GenericTestResult
    {
        public GenericTestResultId Id { get; init; }
        public TestId TestId { get; init; }
        public string Name { get; init; }
        public string Text { get; init; }
        public string? ImagePath { get; init; }
        public virtual ICollection<GenericTestAnswer> AnswersLeadingToResult { get; protected set; } = [];

        public static GenericTestResult CreateNew(TestId testId, string name, string text, string? imagePath) => new() {
            Id = new(),
            Name = name,
            TestId = testId,
            Text = text,
            ImagePath = imagePath
        };


    }
}
