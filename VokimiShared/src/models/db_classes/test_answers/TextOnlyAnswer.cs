namespace VokimiShared.src.models.db_classes.test_answers
{
    public class TextOnlyAnswer : BaseAnswer
    {
        public string Text { get; init; }
        public static TextOnlyAnswer CreateNew(
            DraftTestQuestionId questionId,
            ushort order,
            string text) =>
               new() {
                   AnswerId = new(),
                   QuestionId = questionId,
                   OrderInQuestion = order,
                   Text = text,
               };
    }
}
