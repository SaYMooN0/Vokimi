using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.db_entities_relations_classes
{
    public class RelationsDraftGenericTestAnswerResultData
    {
        public DraftTestTypeSpecificResultDataId DraftTestResultDataId { get; init; }
        public DraftTestAnswerId DraftTestAnswerId { get; init; }

        public virtual DraftGenericTestResultData DraftTestResultData { get; init; }
        public virtual DraftGenericTestAnswer DraftTestAnswer { get; init; }
    }
}
