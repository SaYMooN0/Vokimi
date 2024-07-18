namespace VokimiShared.src.models.db_classes.test_answers
{
    public class DraftTestTextAndImageAnswer : BaseDraftTestAnswer
    {
        public string Text { get; init; }
        public string ImagePath { get; init; }
        public static DraftTestTextAndImageAnswer CreateNew(
            DraftTestQuestionId questionId,
            ushort order,
            string text,
            string imagePath) =>
                new() {
                    AnswerId = new(),
                    QuestionId = questionId,
                    OrderInQuestion = order,
                    Text = text,
                    ImagePath = imagePath
                };

   
    }
}
