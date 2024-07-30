using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_creation.generic_test_related
{
    public class DraftGenericTestAnswer
    {
        public DraftTestAnswerId Id { get; init; }
        public DraftTestQuestionId QuestionId { get; init; }
        public ushort OrderInQuestion { get; set; }
        public virtual ICollection<DraftGenericTestResultData> RelatedResultsData { get; private set; } = [];
        public AnswerTypeSpecificInfoId AdditionalInfoId { get; init; }
        public virtual AnswerTypeSpecificInfo AdditionalInfo { get; private set; }
        public static DraftGenericTestAnswer CreateNew(
            DraftTestQuestionId questionId, ushort orderInQuestion, AnswerTypeSpecificInfoId typeSpecificInfoId) => new() {
            Id= new (),
            QuestionId= questionId,
            OrderInQuestion= orderInQuestion,
            AdditionalInfoId= typeSpecificInfoId
        };
    }

}
