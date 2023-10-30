using System.Security.Claims;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels;

namespace VokimiServices
{
    public interface IDataBase
    {
        public Task<int> AddUser(User user);
        public Task<bool> AnyUserWithSuchEmail(string email);
        public Task<UserProfileViewModel?> GetUserInfo(int id);
        public Task<MyAccountViewModel?> GetMyAccountInfo(int userId);
        public Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);

    }
}
