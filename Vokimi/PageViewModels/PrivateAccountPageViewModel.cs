using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_classes;

namespace Vokimi.PageViewModels
{
    public class PrivateAccountPageViewModel
    {
        public AppUserId PageOwnerId { get; set; }
        public AppUserId? ViewerId { get; set; }
        public string Username { get; set; }
        public string ProfilePicturePath { get; set; }
        public static PrivateAccountPageViewModel FromUser(AppUser pageOwner, AppUserId? viewerId) =>
            new()
            {
                PageOwnerId = pageOwner.Id,
                ViewerId = viewerId,
                Username = pageOwner.Username,
                ProfilePicturePath = pageOwner.ProfilePicturePath
            };
    }
}
