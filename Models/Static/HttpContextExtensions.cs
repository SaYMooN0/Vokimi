namespace Vokimi.Models.Static
{
    public static class HttpContextExtensions
    {
        public static int GetUserIdFromIdentity(this HttpContext httpContext)
        {
            var identity = httpContext.User.Identities.FirstOrDefault(i => i.Claims.Any(c => c.Type == "userId"));
            if (identity != null)
            {
                var claim = identity.Claims.FirstOrDefault(c => c.Type == "userId");
                if (claim != null && int.TryParse(claim.Value, out int userId))
                    return userId;
            }
            return -1;
        }
    }

}
