using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.db_classes.test_answers
{
    public abstract class BaseAnswer
    {
        public AnswerId AnswerId { get; set; }
        public DraftTestQuestionId QuestionId { get; set; }
        public ushort OrderInQuestion { get; set; }
        public virtual ICollection<DraftTestResult> RelatedResults { get; set; } = [];
    }

}
