using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.PageViewModels
{
    public class UserAccountPageViewModel
    {
        public AppUserId PageOwnerId { get; set; }
        public AppUserId? ViewerId { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string ProfilePicturePath { get; set; }
        public static UserAccountPageViewModel FromUser(AppUser pageOwner, AppUserId? viewerId) =>
            new()
            {
                PageOwnerId = pageOwner.Id,
                ViewerId = viewerId,
                Username = pageOwner.Username,
                RealName = pageOwner.RealName,
                ProfilePicturePath = pageOwner.ProfilePicturePath
            };
    }
}
