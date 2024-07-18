namespace VokimiShared.src.models.db_classes.test_answers
{
    public class DraftTestImageOnlyAnswer : BaseDraftTestAnswer
    {
        public string ImagePath { get; init; }
        public static DraftTestImageOnlyAnswer CreateNew(
            DraftTestQuestionId questionId,
            ushort order,
            string imagePath) =>
                new() {
                    AnswerId = new(),
                    QuestionId = questionId,
                    OrderInQuestion = order,
                    ImagePath = imagePath
                };

    }
}
