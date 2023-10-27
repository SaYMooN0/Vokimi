namespace Vokimi.Models.DataBaseClasses
{
    public class User
    {
        public User(string name, string email, string password, DateTime birthDate, bool isBanned, Role role, DateTime registrationDate, string? status, List<uint> commentsId, List<uint> takenTestsId, List<uint> ratedTestsId, List<uint> createdTestsId, List<uint> pinnedTestsId, HashSet<Language> preferredLanguages)
        {
            Name = name;
            Email = email;
            Password = password;
            BirthDate = birthDate;
            IsBanned = isBanned;
            Role = role;
            RegistrationDate = registrationDate;
            Status = status;
            CommentsId = commentsId;
            TakenTestsId = takenTestsId;
            RatedTestsId = ratedTestsId;
            CreatedTestsId = createdTestsId;
            PinnedTestsId = pinnedTestsId;
            PreferredLanguages = preferredLanguages;
        }
        public User(string name, string email, string password, DateTime birthDate, HashSet<Language> preferredLanguages) : this(
            name,
            email,
            password,
            birthDate,
            false,
            Role.None,
            DateTime.Now,
            null,
            new List<uint>(), new List<uint>(), new List<uint>(), new List<uint>(), new List<uint>(),
            preferredLanguages)
        {
        }
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime BirthDate { get; private set; }
        public bool IsBanned { get; private set; }
        public Role Role { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public string? Status { get; private set; }
        public List<uint> CommentsId { get; private set; } = new();
        public List<uint> TakenTestsId { get; private set; } = new();
        public List<uint> RatedTestsId { get; private set; } = new();
        public List<uint> CreatedTestsId { get; private set; } = new();
        public List<uint> PinnedTestsId { get; private set; } = new();
        public HashSet<Language> PreferredLanguages { get; private set; } = new();
        public void Ban() => IsBanned = true;
        public void UnBan() => IsBanned = false;
    }
}
