namespace Shared.src.models.db_classes
{
    public readonly record struct UserId(Guid Value)
    {
        public UserId() : this(new Guid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct UnconfirmedAppUserId(Guid Value)
    {
        public UnconfirmedAppUserId() : this(new Guid()) { }
        public override string ToString() => Value.ToString();
    }
    public readonly record struct LoginInfoId(Guid Value)
    {
        public LoginInfoId() : this(new Guid()) { }
        public override string ToString() => Value.ToString();
    }
}
