namespace Vokimi.Models.DataBaseClasses
{
    public class User
    {
        public User(string name, string email, string password, DateOnly birthDate, bool isBanned, Role role, DateOnly registrationDate, string? status, List<int> commentsId, List<int> takenTestsId, List<int> ratedTestsId, List<int> createdTestsId, List<int> pinnedTestsId, HashSet<Language> preferredLanguages)
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
        public User(string name, string email, string password, DateOnly birthDate) : this(
            name,
            email,
            password,
            birthDate,
            false,
            Role.None,
            DateOnly.FromDateTime(DateTime.Now),
            null,
            new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new HashSet<Language>())
        {
        }
        public User() { }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public bool IsBanned { get; private set; }
        public Role Role { get; private set; }
        public DateOnly RegistrationDate { get; private set; }
        public string? Status { get; private set; }
        public List<int> CommentsId { get; private set; } = new();
        public List<int> TakenTestsId { get; private set; } = new();
        public List<int> RatedTestsId { get; private set; } = new();
        public List<int> CreatedTestsId { get; private set; } = new();
        public List<int> PinnedTestsId { get; private set; } = new();
        public HashSet<Language> PreferredLanguages { get; private set; } = new();
        public void Ban() => IsBanned = true;
        public void UnBan() => IsBanned = false;
    }
}
