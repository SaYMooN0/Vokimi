using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.form_classes;

namespace VokimiShared.src.models.db_classes.tests
{
    public class TestConclusion
    {
        public TestConclusionId Id { get; init; }

        public string Text { get; private set; }
        public string? AdditionalImage { get; private set; }
        public bool Feedback { get; private set; }
        public string FeedbackText { get; private set; }
        public uint MaxCharactersForFeedback { get; private set; }
        public static TestConclusion CreateNew(ConclusionCreationForm data) => new() {
            Id = new(),
            Text = data.Text,
            AdditionalImage = data.ImagePath,
            Feedback = data.AddFeedback,
            FeedbackText = data.FeedbackText,
            MaxCharactersForFeedback = data.MaxCharactersForFeedback
        };
        public void Update(ConclusionCreationForm data) {
            Text = data.Text;
            AdditionalImage = data.ImagePath;
            Feedback = data.AddFeedback;
            FeedbackText = data.FeedbackText;
            MaxCharactersForFeedback = data.MaxCharactersForFeedback;
        }

    }
}
