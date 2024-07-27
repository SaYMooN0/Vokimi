namespace VokimiShared.src.models.db_entities_ids
{
    public readonly record struct DraftTestId(Guid Value)
    {
        public DraftTestId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestMainInfoId(Guid Value)
    {
        public DraftTestMainInfoId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestAnswerId(Guid Value)
    {
        public DraftTestAnswerId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestQuestionId(Guid Value)
    {
        public DraftTestQuestionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestResultId(Guid Value)
    {
        public DraftTestResultId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestTypeSpecificResultDataId(Guid Value)
    {
        public DraftTestTypeSpecificResultDataId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
}
