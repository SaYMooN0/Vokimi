namespace VokimiShared.src.models.db_classes.answers
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
