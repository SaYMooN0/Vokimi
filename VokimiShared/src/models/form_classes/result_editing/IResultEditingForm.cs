namespace VokimiShared.src.models.form_classes.result_editing
{
    public interface IResultEditingForm
    {
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public string GetResultStringId();
    }
}
