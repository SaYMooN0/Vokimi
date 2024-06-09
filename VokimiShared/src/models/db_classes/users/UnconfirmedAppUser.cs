namespace VokimiShared.src.models.db_classes.users
{
    public class UnconfirmedAppUser
    {
        public UnconfirmedAppUserId Id { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public DateTime RegistrationDate { get; init; }
        public required string LoginInfo { get; init; }
        public required string ConfirmationCode { get; init; }
    }
}
