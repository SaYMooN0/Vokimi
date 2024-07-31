using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.db_entities_relations_classes
{
    public class GenericTestAnswerLeadingToResultRelations
    {
        public GenericTestAnswerId GenericTestAnswerId { get; init; }
        public GenericTestResultId GenericTestResultId { get; init; }
        public virtual GenericTestAnswer GenericTestAnswer { get; init; }
        public virtual GenericTestResult GenericTestResult { get; init; }
    }
}
