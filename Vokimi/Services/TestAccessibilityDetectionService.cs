using System.Security.AccessControl;
using System.Security.Claims;
using Vokimi.src.data;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.Services
{
    public class TestAccessibilityDetectionService
    {
        private readonly AuthHelperService _authHelper;
        public TestAccessibilityDetectionService(AuthHelperService authHelper) {
            _authHelper = authHelper;
        }
        internal TestAccessibility DoesUserHaveAccess(ClaimsPrincipal claims, VokimiDbContext dbContext, BaseTest test) {
            if (test.Privacy == TestPrivacy.Anyone) {
                return TestAccessibility.Accessible;
            }

            AppUserId? viewerId = _authHelper.GetUserIdFromClaims(claims);
            if (viewerId == test.CreatorId) {
                return TestAccessibility.Accessible;
            }

            switch (test.Privacy) {
                case TestPrivacy.FriendsOnly:
                    if (test.Creator.Friends.Any(u => u.Id == viewerId)) {
                        return TestAccessibility.Accessible;
                    }
                    return TestAccessibility.FriendshipNeeded;

                case TestPrivacy.FriendsAndFollowers:
                    if (test.Creator.Friends.Any(u => u.Id == viewerId) || test.Creator.Followers.Any(u => u.Id == viewerId)) {
                        return TestAccessibility.Accessible;
                    }
                    return TestAccessibility.FollowingNeeded;

                default:
                    return TestAccessibility.NotAccessible;
            }
        }
        internal bool IsViewerCreator(ClaimsPrincipal claims, VokimiDbContext dbContext, BaseTest test) {
            AppUserId? viewerId = _authHelper.GetUserIdFromClaims(claims);
            return viewerId == test.CreatorId;
        }
        internal enum TestAccessibility
        {
            Accessible,
            FollowingNeeded,
            FriendshipNeeded,
            NotAccessible
        }

    }
}
