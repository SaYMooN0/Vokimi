using Dapper;
using System.Data.SqlClient;
using System.Security.Claims;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels;

namespace Vokimi.Services.Classes
{
    public class DataBase : VokimiServices.IDataBase
    {
        private readonly string _connectionString;

        public DataBase(string connectionString) { _connectionString = connectionString; }
        public async Task<int> AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string insertUserQuery = @"INSERT INTO Users (Name, Email, Password, BirthDate, IsBanned, Role, RegistrationDate, Status) 
                                   VALUES (@Name, @Email, @Password, @BirthDate, @IsBanned, @Role, @RegistrationDate, @Status);
                                   SELECT CAST(SCOPE_IDENTITY() as int)";
                var userId = await connection.QuerySingleAsync<int>(insertUserQuery, user);
                return userId;
            }
        }
        public async Task<bool> AnyUserWithSuchEmail(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string checkEmailQuery = @"SELECT COUNT(1) FROM Users WHERE Email = @Email";
                int count = await connection.QuerySingleAsync<int>(checkEmailQuery, new { Email = email });

                return count > 0;
            }
        }

        public async Task<UserProfileViewModel?> GetUserInfo(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                User? user = await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @UserId",
                    new { UserId = userId });
                if (user is null)
                    return null;

                UserProfileViewModel viewModel = new(userId, user.Name, user.Status);

                var lastTakenTest = await connection.QueryFirstOrDefaultAsync<int?>(
                    @"SELECT TOP 1 TestId FROM TestsTakings WHERE UserId = @UserId ORDER BY TakingDate DESC",
                    new { UserId = userId });

                viewModel.LastTakenTest = lastTakenTest;
                viewModel.CreatedTests = (await connection.QueryAsync<int>(
                    "SELECT Id FROM Tests WHERE AuthorId = @UserId",
                    new { UserId = userId })).ToList();

                return viewModel;
            }
        }
        public async Task<MyAccountViewModel?> GetMyAccountInfo(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                User? user = await connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @UserId",
                    new { UserId = userId });

                if (user is null)
                    return null;

                MyAccountViewModel viewModel = new(userId, user.Name, user.Email, user.Status);

                viewModel.TakenTests = (await connection.QueryAsync<int>(
                    "SELECT TestId FROM TestsTakings WHERE UserId = @UserId ORDER BY TakingDate DESC",
                    new { UserId = userId })).ToList();

                viewModel.LastTakenTest = viewModel.TakenTests.FirstOrDefault();

                viewModel.CreatedTests = (await connection.QueryAsync<int>(
                    "SELECT Id FROM Tests WHERE AuthorId = @UserId", new { UserId = userId })).ToList();

                viewModel.RatedTests = (await connection.QueryAsync<int>(
                    "SELECT TestId FROM TestsRatings WHERE UserId = @UserId", new { UserId = userId })).ToList();

                viewModel.PinnedTests = (await connection.QueryAsync<int>(
                    "SELECT TestId FROM PinnedTests WHERE UserId = @UserId",
                    new { UserId = userId })).ToList();

                return viewModel;
            }
        }
        async public Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string userSelectString = @"SELECT * FROM Users WHERE Email = @Email AND Password = @password";
                return await connection.QuerySingleOrDefaultAsync<User>(userSelectString, new { Email = email, Password = password });
            }
        }


    }
}
