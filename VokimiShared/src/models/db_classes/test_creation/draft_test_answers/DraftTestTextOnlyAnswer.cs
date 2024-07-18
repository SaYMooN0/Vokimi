namespace VokimiShared.src.models.db_classes.test_answers
{
    public class DraftTestTextOnlyAnswer : BaseDraftTestAnswer
    {
        public string Text { get; init; }
        public static DraftTestTextOnlyAnswer CreateNew(
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
