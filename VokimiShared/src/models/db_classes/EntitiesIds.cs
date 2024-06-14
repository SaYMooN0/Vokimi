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

    public readonly record struct UnpublishedTestId(Guid Value)
    {
        public UnpublishedTestId() : this(Guid.NewGuid()) { }
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
}
