using VokimiShared.src.constants_store_classes;

namespace VokimiShared.src.models.form_classes
{
    public class ConclusionCreationForm
    {
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public bool AddFeedback { get; set; }
        public string? FeedbackText { get; set; } = "Please share your impression of the test";
        public uint? MaxCharactersForFeedback { get; set; } = 120;
    }
}
