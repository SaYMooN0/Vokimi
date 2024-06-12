namespace VokimiShared.src.models.db_classes.users
{
    public class LoginInfo
    {
        public LoginInfoId Id { get; private set; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
        public static LoginInfo CreateNew(string email, string passwordHash) =>
            new()
            {
                Id = new LoginInfoId(),
                Email = email,
                PasswordHash = passwordHash
            };
    }
}
