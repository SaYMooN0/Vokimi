namespace VokimiShared.src.models.form_classes.result_editing
{
    public class NotSavedResultForm : IResultEditingForm
    {
        public string ResultStringId { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }

        public string GetResultStringId() => ResultStringId;
        public static NotSavedResultForm Empty => new() {
            ResultStringId = string.Empty,
            Text = string.Empty,
            ImagePath = null,
        };
    }
}
