namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public class TextOnlyAnswerForm : BaseAnswerForm
    {
        public string Text { get; set; }
        public override Err Validate() {
            if (string.IsNullOrWhiteSpace(Text))
                return new Err("Text cannot be empty");
            return Err.None;
        }
    }
}
