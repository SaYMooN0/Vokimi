namespace VokimiShared.src.models.db_classes.generic_test_answers
{
    public class TextOnlyAnswerAdditionalInfo : AnswerTypeSpecificInfo
    {
        public string Text { get; set; }
        public static TextOnlyAnswerAdditionalInfo CreateNew(string text) => new() {
            Id = new(),
            Text = text,
        };
    }
}
