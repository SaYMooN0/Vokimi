using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.users
{
    public class AppUser
    {
        public AppUserId Id { get; private set; }
        public string Username { get; private set; }
        public string RealName { get; private set; } = string.Empty;
        public string ProfilePicturePath { get; private set; } = DefaultProfilePicturePath;
        public bool IsAccountPrivate { get; private set; } = false;
        public LoginInfoId LoginInfoId { get; init; }
        public UserAdditionalInfoId UserAdditionalInfoId { get; init; }

        public virtual LoginInfo LoginInfo { get; private set; }
        public virtual UserAdditionalInfo UserAdditionalInfo { get; private set; }

        public virtual ICollection<BaseDraftTest> DraftTests { get; set; } = [];
        public static AppUser CreateNew(string username, LoginInfoId loginInfoId, UserAdditionalInfoId userAdditionalInfoId) =>
            new() {
                Id = new AppUserId(),
                Username = username,
                RealName = string.Empty,
                ProfilePicturePath = DefaultProfilePicturePath,
                IsAccountPrivate = false,
                LoginInfoId = loginInfoId,
                UserAdditionalInfoId = userAdditionalInfoId
            };

        public const string DefaultProfilePicturePath = "default.webp";

    }
}
