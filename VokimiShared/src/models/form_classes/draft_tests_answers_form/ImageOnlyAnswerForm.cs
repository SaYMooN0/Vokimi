namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public class ImageOnlyAnswerForm : BaseAnswerForm, IAnswerFormWithImage
    {
        public string ImagePath { get; set; }
        public override Err Validate() {
            if (string.IsNullOrWhiteSpace(ImagePath))
                return new Err("Choose an image");
            return Err.None;
        }
    }
}
