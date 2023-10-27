using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Services.Interfaces
{
    public interface IDataBase
    {
        public Task<int> AddUser(User user);
    }
}
