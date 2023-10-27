using System.Runtime.CompilerServices;

namespace Vokimi.Models.DataBaseClasses
{
    public class User
    {
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
        public HashSet<Language> PreferredLanguages { get; private set; }
        public void Ban() => IsBanned = true;
        public void UnBan() => IsBanned = false;
    }
}
