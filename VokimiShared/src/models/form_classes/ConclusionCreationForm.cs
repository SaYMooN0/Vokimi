
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.form_classes
{
    public class ConclusionCreationForm
    {
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public bool AddFeedback { get; set; }
        public string FeedbackText { get; set; } = "Please share your impression of the test";
        public uint MaxCharactersForFeedback { get; set; } = 120;
        public static ConclusionCreationForm FromConclusion(TestConclusion conclusion) => new() {
            Text = conclusion.Text,
            ImagePath = conclusion.AdditionalImage,
            AddFeedback = conclusion.Feedback,
            FeedbackText = conclusion.FeedbackText,
            MaxCharactersForFeedback = conclusion.MaxCharactersForFeedback,
        };
    }

}
