using VokimiShared.src.models.db_classes.answers;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.db_classes.test_answers
{
    public class DraftTestAnswer
    {
        public DraftTestAnswerId Id { get; init; }
        public DraftTestQuestionId QuestionId { get; init; }
        public ushort OrderInQuestion { get; set; }
        public virtual ICollection<DraftTestResult> RelatedResults { get; private set; } = [];
        public AnswerTypeSpecificInfoId AdditionalInfoId { get; init; }
        public virtual AnswerTypeSpecificInfo AdditionalInfo { get; private set; }
        public static DraftTestAnswer CreateNew(
            DraftTestQuestionId questionId, ushort orderInQuestion, AnswerTypeSpecificInfoId typeSpecificInfoId) => new() {
            Id= new (),
            QuestionId= questionId,
            OrderInQuestion= orderInQuestion,
            AdditionalInfoId= typeSpecificInfoId
        };
    }

}
