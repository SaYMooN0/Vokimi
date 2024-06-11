namespace VokimiShared.src.models.db_classes.users
{
    public class AppUser
    {
        public AppUserId Id { get; private set; }
        public string Username { get; init; }
        public LoginInfoId LoginInfoId { get; private set; }

        public virtual LoginInfo LoginInfo { get; set; }

        public AppUser(string username, LoginInfoId loginInfoId)
        {
            Id = new();
            Username = username;
            LoginInfoId = loginInfoId;
        }



    }
}
