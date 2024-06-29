namespace VokimiShared.src.models.db_classes.test_answers
{
    public class TextOnlyAnswer : BaseAnswer
    {
        public string Text { get; init; }
        public static TextOnlyAnswer CreateNew(
            DraftTestQuestionId questionId,
            int points,
            ushort order,
            string text) =>
               new() {
                   AnswerId = new(),
                   QuestionId = questionId,
                   Points = points,
                   OrderInQuestion = order,
                   Text = text,
               };
    }
}
