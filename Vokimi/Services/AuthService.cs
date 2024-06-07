using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

namespace Vokimi.Services
{
    public class AuthService
    {
        public AuthService()
        {
            throw new NotImplementedException();

        }

        public async Task<IdentityResult> Register(string email, string name, string password)
        {
            throw new NotImplementedException();

        }
        public async Task<SignInResult> LoginUserAsync(string email, string password)
        {
            throw new NotImplementedException();

        }
    }
}
