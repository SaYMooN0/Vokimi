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
        public const string AuthCookieName ="auth_token";
        public ClaimsPrincipal CreateUserPrincipal(string email, AppUserId id) //TODO: only user, add role
        {
            List<Claim> claims = [
                new(type: "email",email),
                new (type: "id", id.ToString())
            ];

            var identity = new ClaimsIdentity(claims, AuthScheme);
            return new ClaimsPrincipal(identity);

        }
        //public async Task<Err> SignInAsync(string email, string password, bool checkPassword = true)
        //{
        //    AppUser? user = await _usersDbOperationsService.GetUserByEmail(email);
        //    if (user is null)
        //        return new("There is no account with this email");
        //    if (checkPassword)
        //        if (!BCrypt.Net.BCrypt.Verify(password, user.LoginInfo.PasswordHash))
        //            return new("Invalid password");

        //    List<Claim> claims = [
        //        new(type: "email", user.LoginInfo.Email),
        //        new (type: "id", user.Id.ToString())
        //    ];

        //    var identity = new ClaimsIdentity(claims, AuthScheme);
        //    var principal = new ClaimsPrincipal(identity);

        //    await _httpContextAccessor.HttpContext.SignInAsync(AuthScheme, principal);
        //    return Err.None;
        //}
        //public AppUserId? GetAppUserIdFromIdentity()
        //{
        //    ClaimsIdentity claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        //    if (claimsIdentity is not null)
        //    {
        //        var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "id");
        //        if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
        //            return new AppUserId(userId);
        //    }
        //    return null;
        //}
        
        public bool IsSignedIn(AuthenticationState? state) =>
            state is not null && state.User is not null && state.User.Identity.IsAuthenticated;

    }
}
