using Vokimi.Services.Interfaces;

namespace Vokimi.Services.Classes
{
    public class DataBase:IDataBase
    {
        private readonly string _connectionString;

        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
