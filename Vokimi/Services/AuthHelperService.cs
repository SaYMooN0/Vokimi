using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using VokimiShared.src;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.users;

namespace Vokimi.Services
{
    public class AuthHelperService
    {
        public const string AuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string AuthCookieName = "auth_token";
        public ClaimsPrincipal CreateUserPrincipal(string email, AppUserId id) //TODO: only user, add role
        {
            List<Claim> claims = [
                new(type: "email",email),
                new (type: "id", id.ToString())
            ];

            var identity = new ClaimsIdentity(claims, AuthScheme);
            return new ClaimsPrincipal(identity);

        }
        public AppUserId? GetUserIdFromClaims(ClaimsPrincipal claims)
        {
            if (claims is null) return null;

            string? idStr = claims.FindFirst("id")?.Value;

            if (Guid.TryParse(idStr, out Guid guid))
                return new AppUserId(guid);

            return null;
        }

        public bool IsSignedIn(AuthenticationState? state) =>
            state is not null && state.User is not null && state.User.Identity.IsAuthenticated;

    }
}
