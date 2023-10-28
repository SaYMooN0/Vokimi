using Vokimi.Models.DataBaseClasses;

namespace VokimiServices
{
    public interface IDataBase
    {
        public Task<int> AddUser(User user);
        public Task<bool> AnyUserWithSuchEmail(string email);

    }
}
