using VokimiShared.src.models.db_classes.questions;

namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public abstract class BaseAnswerForm
    {
        public int Points { get; set; }
    }
    internal static class BaseAnswerFormFabricExtensions
    {
        public static TextOnlyAnswerForm ToBaseAnswer(this TextOnlyAnswer baseAnswerForm) => new TextOnlyAnswerForm {
            Points = baseAnswerForm.Points,
            Text = baseAnswerForm.Text
        };

    }
}
