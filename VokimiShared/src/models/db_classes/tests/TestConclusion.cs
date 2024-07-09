using VokimiShared.src.models.form_classes;

namespace VokimiShared.src.models.db_classes.tests
{
    public class TestConclusion
    {
        public TestConclusionId Id { get; init; }

        public string Text { get; init; }
        public string? AdditionalImage { get; init; }
        public bool Feedback { get; init; }
        public string? FeedbackText { get; init; }
        public uint? MaxCharactersForFeedback { get; init; }
        public static TestConclusion CreateNew(ConclusionCreationForm data) => new() {
            Id = new(),
            Text = data.Text,
            AdditionalImage = data.ImagePath,
            Feedback = data.AddFeedback,
            FeedbackText = data.FeedbackText,
            MaxCharactersForFeedback = data.MaxCharactersForFeedback
        };
    }
}
