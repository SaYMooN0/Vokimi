namespace Shared.src.models.db_classes.users
{
    public class LoginInfo
    {
        public LoginInfoId Id { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
