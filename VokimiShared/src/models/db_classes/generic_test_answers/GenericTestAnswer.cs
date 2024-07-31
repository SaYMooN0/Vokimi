using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.generic_test_answers
{
    public class GenericTestAnswer
    {
        public GenericTestAnswerId Id { get; init; }
        public GenericTestQuestionId QuestionId { get; init; }
        public ushort OrderInQuestion { get; set; }
        public virtual ICollection<GenericTestResult> RelatedResults{ get; protected set; } = [];
        public AnswerTypeSpecificInfoId AdditionalInfoId { get; init; }
        public virtual AnswerTypeSpecificInfo AdditionalInfo { get; protected set; }
       
    }
}
