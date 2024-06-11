namespace VokimiShared.src.models.db_classes.users
{
    public class LoginInfo
    {
        public LoginInfoId Id { get; private set; }
        public string Email { get; init; }
        public string PasswordHash { get; init; }
        public LoginInfo(string email, string passwordHash)
        {
            Id = new LoginInfoId();
            Email = email;
            PasswordHash = passwordHash;
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}
