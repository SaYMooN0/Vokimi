namespace VokimiShared.src.models.form_classes
{
    public class QuestionEditingForm
    {
        public string Text { get; init; }
        public string? ImagePath { get; init; }

        public bool IsMultipleChoice { get; init; }
        public ushort MinAnswersCount { get; init; }
        public ushort MaxAnswersCount { get; init; }
        public static QuestionEditingForm NewDefault() => new() {
            Text = "New question",
            IsMultipleChoice = false,
            MinAnswersCount = 0,
            MaxAnswersCount = 1
        };
    }
}
