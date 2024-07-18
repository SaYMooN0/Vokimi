namespace VokimiShared.src.models.db_classes
{
    public readonly record struct AppUserId(Guid Value)
    {
        public AppUserId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct UnconfirmedAppUserId(Guid Value)
    {
        public UnconfirmedAppUserId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct LoginInfoId(Guid Value)
    {
        public LoginInfoId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct UserAdditionalInfoId(Guid Value)
    {
        public UserAdditionalInfoId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }

    public readonly record struct DraftTestMainInfoId(Guid Value)
    {
        public DraftTestMainInfoId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct DraftTestId(Guid Value)
    {
        public DraftTestId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct TestConclusionId(Guid Value)
    {
        public TestConclusionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct TestTagId(Guid Value)
    {
        public TestTagId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct QuestionId(Guid Value)
    {
        public QuestionId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct AnswerId(Guid Value)
    {
        public AnswerId() : this(Guid.NewGuid()) { }
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
    public readonly record struct TestStylesSheetId(Guid Value)
    {
        public TestStylesSheetId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct TestId(Guid Value)
    {
        public TestId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    } 
    public readonly record struct MultiChoiceQuestionDataId(Guid Value)
    {
        public MultiChoiceQuestionDataId() : this(Guid.NewGuid()) { }
        public override string ToString() => Value.ToString();
    }
}
