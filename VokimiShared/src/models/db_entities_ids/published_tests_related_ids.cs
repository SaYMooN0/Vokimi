
namespace VokimiShared.src.models.db_entities_ids
{
    public readonly record struct TestId(Guid Value)
    {
        public TestId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct QuestionId(Guid Value)
    {
        public QuestionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct MultiChoiceQuestionDataId(Guid Value)
    {
        public MultiChoiceQuestionDataId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct TestTagId(Guid Value)
    {
        public TestTagId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
}
