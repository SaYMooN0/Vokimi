namespace VokimiShared.src.models.db_classes.test_answers
{
    public abstract class BaseAnswer
    {
        public AnswerId AnswerId { get; set; }
        public int Points { get; init; }
        public DraftTestQuestionId QuestionId { get; set; }
        public ushort OrderInQuestion { get; set; }

    }
    
}
