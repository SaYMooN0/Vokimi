using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_results.results_for_draft_tests
{
    public abstract class DraftTestTypeSpecificResultData
    {
        public DraftTestTypeSpecificResultDataId Id { get; init; }

        public virtual DraftTestResult DraftTestResult { get; set; }
    }
}
