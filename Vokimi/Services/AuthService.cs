using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using VokimiShared.src;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.users;

namespace Vokimi.Services
{
    public class AuthService
    {
        private readonly UsersDbOperationsService _usersDbOperationsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UsersDbOperationsService usersDbOperationsService,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _usersDbOperationsService = usersDbOperationsService;
            _httpContextAccessor = httpContextAccessor;
        }
        public const string AuthCookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string AuthCookieName ="auth";
        public async Task<Err> SignInAsync(string email, string password, bool checkPassword = true)
        {
            AppUser? user = await _usersDbOperationsService.GetUserByEmail(email);
            if (user is null)
                return new("There is no account with this email");
            if (checkPassword)
                if (!BCrypt.Net.BCrypt.Verify(password, user.LoginInfo.PasswordHash))
                    return new("Invalid password");

            List<Claim> claims = [
                new(type: "email", user.LoginInfo.Email),
                new (type: "id", user.Id.ToString())
            ];

            var identity = new ClaimsIdentity(claims, AuthCookieScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(AuthCookieScheme, principal);
            return Err.None;
        }
        public AppUserId? GetAppUserIdFromIdentity()
        {
            ClaimsIdentity claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity is not null)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "id");
                if (userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out var userId))
                    return new AppUserId(userId);
            }
            return null;
        }
        public async Task SignOutAsync() =>
            await _httpContextAccessor.HttpContext.SignOutAsync(AuthCookieScheme);

        public async Task<bool> IsSignedInAsync()
        {
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(AuthCookieScheme);
            if (authenticateResult.Succeeded && authenticateResult.Principal is not null)
            {
                AppUserId? userId = GetAppUserIdFromIdentity();
                if (userId.HasValue)
                {
                    var user = await _usersDbOperationsService.GetUserByIdAsync(userId.Value);
                    if (user is not null)
                        return true;
                }
                await SignOutAsync();
            }
            return false;
        }

    }
}
