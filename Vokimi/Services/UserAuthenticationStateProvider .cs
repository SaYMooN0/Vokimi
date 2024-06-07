using Microsoft.AspNetCore.Components.Authorization;

namespace Vokimi.Services
{
    public class UserAuthenticationStateProvider : AuthenticationStateProvider
    {
        public UserAuthenticationStateProvider()
        {
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task MarkUserAsAuthenticated(string email)
        {
            throw new NotImplementedException();
        }

        public void MarkUserAsLoggedOut()
        {
            throw new NotImplementedException();

        }
    }
}
