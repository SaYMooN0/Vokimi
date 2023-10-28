using Dapper;
using System.Data.SqlClient;
using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Services.Classes
{
    public class DataBase: VokimiServices.IDataBase
    {
        private readonly string _connectionString;

        public DataBase(string connectionString) { _connectionString = connectionString;}
        public async Task<int> AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string insertUserQuery = @"INSERT INTO Users (Name, Email, Password, BirthDate, IsBanned, Role, RegistrationDate, Status) 
                                   VALUES (@Name, @Email, @Password, @BirthDate, @IsBanned, @Role, @RegistrationDate, @Status);
                                   SELECT CAST(SCOPE_IDENTITY() as int)";
                var userId = await connection.QuerySingleAsync<int>(insertUserQuery, user);
                if (user.PreferredLanguages?.Any() == true)
                {
                    string insertLanguagesQuery = @"INSERT INTO UserPreferredLanguages (UserId, Language) VALUES (@UserId, @Language)";
                    foreach (var language in user.PreferredLanguages)
                    {
                        await connection.ExecuteAsync(insertLanguagesQuery, new { UserId = userId, Language = (int)language });
                    }
                }
                return userId;
            }
        }

        public Task<bool> AnyUserWithSuchEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
