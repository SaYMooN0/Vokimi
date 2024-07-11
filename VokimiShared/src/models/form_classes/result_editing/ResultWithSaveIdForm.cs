using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.form_classes.result_editing
{
    public class ResultWithSaveIdForm : IResultEditingForm
    {
        public DraftTestResultId DraftTestResultId { get; init; }
        public string StringId { get; init; }
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public string GetResultStringId() => StringId;
        public static ResultWithSaveIdForm FromDraftTestResult(DraftTestResult result) => new() {
            DraftTestResultId = result.Id,
            StringId = result.StringId,
            Text = result.Text,
            ImagePath = result.ImagePath
        };

    }
}
