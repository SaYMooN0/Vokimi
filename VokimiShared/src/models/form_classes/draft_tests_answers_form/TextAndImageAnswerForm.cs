namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public class TextAndImageAnswerForm : BaseAnswerForm,IAnswerFormWithImage
    {
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public override Err Validate() {
            if (string.IsNullOrWhiteSpace(Text))
                return new Err("Text cannot be empty");
            if (string.IsNullOrWhiteSpace(ImagePath))
                return new Err("Choose an image");
            return Err.None;
        }
    }
}
