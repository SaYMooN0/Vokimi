namespace VokimiShared.src.models.db_classes.generic_test_answers
{
    public  class TextAndImageAnswerAdditionalInfo : AnswerTypeSpecificInfo
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public static TextAndImageAnswerAdditionalInfo CreateNew(string text, string imagePath) => new() {
            Id = new(),
            Text = text,
            ImagePath = imagePath
        };
    }
}
