using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.users
{
    public class AppUser
    {
        public AppUserId Id { get; init; }
        public string Username { get; private set; }
        public string RealName { get; private set; } = string.Empty;
        public string ProfilePicturePath { get; private set; }
        public bool IsAccountPrivate { get; private set; } = false;
        public LoginInfoId LoginInfoId { get; init; }
        public UserAdditionalInfoId UserAdditionalInfoId { get; init; }

        public virtual LoginInfo LoginInfo { get; private set; }
        public virtual UserAdditionalInfo UserAdditionalInfo { get; private set; }

        public virtual ICollection<BaseDraftTest> DraftTests { get;private set; } = [];
        public virtual ICollection<BaseTest> PublishedTests { get; private set; } = [];
        public virtual ICollection<AppUser> Friends { get; private set; } = [];
        public virtual ICollection<AppUser> Followers { get; private set; } = [];
        public static AppUser CreateNew(string username, LoginInfoId loginInfoId, UserAdditionalInfoId userAdditionalInfoId) =>
            new() {
                Id = new AppUserId(),
                Username = username,
                RealName = string.Empty,
                ProfilePicturePath = ImgOperationsHelper.DefaultProfilePicture,
                IsAccountPrivate = false,
                LoginInfoId = loginInfoId,
                UserAdditionalInfoId = userAdditionalInfoId
            };

    }
}
