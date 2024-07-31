
namespace VokimiShared.src.models.db_entities_ids
{
    public readonly record struct TestId(Guid Value)
    {
        public TestId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct GenericTestQuestionId(Guid Value)
    {
        public GenericTestQuestionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct MultiChoiceQuestionDataId(Guid Value)
    {
        public MultiChoiceQuestionDataId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct GenericTestResultId(Guid Value)
    {
        public GenericTestResultId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct GenericTestAnswerId(Guid Value)
    {
        public GenericTestAnswerId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct TestTagId(Guid Value)
    {
        public TestTagId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
}
