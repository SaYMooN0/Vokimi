namespace VokimiShared.src.models.db_entities_ids
{
    public readonly record struct TestConclusionId(Guid Value)
    {
        public TestConclusionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }


    public readonly record struct TestStylesSheetId(Guid Value)
    {
        public TestStylesSheetId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct AnswerTypeSpecificInfoId(Guid Value)
    {
        public AnswerTypeSpecificInfoId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
}
