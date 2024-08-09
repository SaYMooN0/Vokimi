using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.form_classes
{
    public class ResultEditingForm
    {
        public DraftTestResultId Id { get; init; }
        public string? Name { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public static ResultEditingForm FromDraftTestResult(DraftTestResult result) => new() {
            Id = result.Id,
            Name = result.Name,
            Text = result.Text,
            ImagePath = result.ImagePath
        };
        public static ResultEditingForm Empty => new() {
            Id = new(Guid.Empty),
            Name = string.Empty,
            Text = string.Empty,
            ImagePath = string.Empty
        };
    }
}
