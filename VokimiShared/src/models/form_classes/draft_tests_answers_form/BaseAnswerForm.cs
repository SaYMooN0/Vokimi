namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public abstract class BaseAnswerForm
    {
        public int Points { get; set; }
        public abstract Err Validate();
    }
}
