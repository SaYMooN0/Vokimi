namespace VokimiShared.src.models.db_classes.test_answers
{
    public class TextAndImageAnswer : BaseAnswer
    {
        public string Text { get; init; }
        public string ImagePath { get; init; }
        public static TextAndImageAnswer CreateNew(
            DraftTestQuestionId questionId,
            int points,
            ushort order,
            string text,
            string imagePath) =>
                new() {
                    AnswerId = new(),
                    QuestionId = questionId,
                    Points = points,
                    OrderInQuestion = order,
                    Text = text,
                    ImagePath = imagePath
                };

   
    }
}
