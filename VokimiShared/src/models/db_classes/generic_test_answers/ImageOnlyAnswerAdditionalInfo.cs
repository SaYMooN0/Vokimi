namespace VokimiShared.src.models.db_classes.generic_test_answers
{
    public class ImageOnlyAnswerAdditionalInfo : AnswerTypeSpecificInfo
    {
        public string ImagePath { get; set; }
        public static ImageOnlyAnswerAdditionalInfo CreateNew(string imagePath) => new() {
            Id = new(),
            ImagePath = imagePath
        };
    }
}
