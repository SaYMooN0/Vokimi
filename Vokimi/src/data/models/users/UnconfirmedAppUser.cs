namespace Vokimi.src.data.models.users
{
    public class UnconfirmedAppUser
    {
        public UnconfirmedAppUserId Id { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public DateTime RegistrationDate { get; init; }
        public LoginInfo LoginInfo { get; init; }
        public string ConfirmationCode { get; init; }
    }
}
