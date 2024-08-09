using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.form_classes.draft_tests_answers_form
{
    public abstract class BaseAnswerForm
    {
        public Dictionary<DraftTestResultId, string> RelatedResults { get; set; } = [];
        public abstract Err Validate();
    }
}
