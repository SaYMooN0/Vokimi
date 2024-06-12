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
}
