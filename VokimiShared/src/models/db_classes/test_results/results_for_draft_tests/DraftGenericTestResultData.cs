using VokimiShared.src.models.db_classes.test_creation.generic_test_related;

namespace VokimiShared.src.models.db_classes.test_results.results_for_draft_tests
{
    public class DraftGenericTestResultData : DraftTestTypeSpecificResultData
    {
        public virtual ICollection<DraftGenericTestAnswer> AnswersLeadingToResult { get; set; } = [];
        public static DraftGenericTestResultData CreateNew() => new() {
            Id = new(),
            AnswersLeadingToResult = []
        };

    }
}
