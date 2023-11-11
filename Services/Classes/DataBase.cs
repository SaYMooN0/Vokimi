using Dapper;
using System.Data.SqlClient;
using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels.Account;
using Vokimi.Models.ViewModels.Tests;
using VokimiServices;

namespace Vokimi.Services.Classes
{
    public class DataBase : VokimiServices.IDataBase
    {
        private readonly string _connectionString;

        public DataBase(string connectionString) { _connectionString = connectionString; }
        public async Task<int> AddNewUser(User user)
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
        public async Task<int> AddNewTest(Test test)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO Tests (Name, Description, AuthorId, AgeRestriction, Language, CreationTime, ImagePath) 
VALUES (@Name, @Description, @AuthorId, @AgeRestriction, @Language, @CreationTime, @ImagePath);

SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    test.Name,
                    test.Description,
                    test.AuthorId,
                    AgeRestriction = (int)test.AgeRestriction,
                    Language = (int)test.Language,
                    test.CreationTime,
                    test.ImagePath
                };

                connection.Open();
                return await connection.QuerySingleAsync<int>(query, parameters);
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
        async public Task<int> CreateNewTest(TestCreationData testCreationData, int authorId)
        {
            Test newTest =
                new Test(testCreationData.TestName, null, testCreationData.Description, authorId, testCreationData.AgeRestriction, testCreationData.Language, testCreationData.Tags);
            int testId = await AddNewTest(newTest);
            await AddNewResults(testCreationData.Results, testId);
            await AddNewQuestions(testCreationData.Questions, testId);
            await AddTagsForTest(testCreationData.Tags, testId);
            return testId;
        }
        public async Task<int> AddNewResult(Result result, int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO Results (Text, Description, ImagePath, GapMin, GapMax, TestId) 
VALUES (@Text, @Description, @ImagePath, @GapMin, @GapMax, @TestId);

SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    result.Text,
                    result.Description,
                    result.ImagePath,
                    result.GapMin,
                    result.GapMax,
                    TestId = testId
                };

                int insertedId = await connection.QuerySingleAsync<int>(query, parameters);
                return insertedId;
            }
        }
        public async Task<int> AddNewQuestion(Question question, int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO Questions (ImagePath, AnswerOptionString, Text, TestId) 
VALUES (@ImagePath, @AnswerOptionString, @Text, @TestId);

SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    question.ImagePath,
                    AnswerOptionString = question.answerOptionString,
                    question.Text,
                    TestId = testId
                };

                int insertedId = await connection.QuerySingleAsync<int>(query, parameters);
                return insertedId;
            }
        }
        public async Task<int> AddTagForTest(TestTag tag, int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO TestTags (TestId, Tag) 
VALUES (@TestId, @Tag);

SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    TestId = testId,
                    Tag = (int)tag
                };

                int insertedId = await connection.QuerySingleAsync<int>(query, parameters);
                return insertedId;
            }
        }
        public async Task<IEnumerable<int>> AddNewResults(IEnumerable<Result> results, int testId)
        {
            var tasks = results.Select(async result =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
INSERT INTO Results (Text, Description, ImagePath, GapMin, GapMax, TestId) 
VALUES (@Text, @Description, @ImagePath, @GapMin, @GapMax, @TestId);

SELECT CAST(SCOPE_IDENTITY() as int);";

                    var parameters = new
                    {
                        result.Text,
                        result.Description,
                        result.ImagePath,
                        result.GapMin,
                        result.GapMax,
                        TestId = testId
                    };

                    connection.Open();
                    return await connection.QuerySingleAsync<int>(query, parameters);
                }
            });

            var insertedIds = await Task.WhenAll(tasks);
            return insertedIds;
        }
        public async Task<IEnumerable<int>> AddNewQuestions(IEnumerable<Question> questions, int testId)
        {
            var tasks = questions.Select(async question =>
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
INSERT INTO Questions (ImagePath, AnswerOptionString, Text, TestId) 
VALUES (@ImagePath, @AnswerOptionString, @Text, @TestId);

SELECT CAST(SCOPE_IDENTITY() as int);";

                    var parameters = new
                    {
                        question.ImagePath,
                        AnswerOptionString = question.answerOptionString,
                        question.Text,
                        TestId = testId
                    };

                    connection.Open();
                    return await connection.QuerySingleAsync<int>(query, parameters);
                }
            });

            var insertedIds = await Task.WhenAll(tasks);
            return insertedIds;
        }
        async public Task<IEnumerable<int>> AddTagsForTest(IEnumerable<TestTag> tags, int testId)
        {
            List<int> ids = new();
            foreach (TestTag tag in tags)
            {
                ids.Add(await AddTagForTest(tag, testId));
            }
            return ids;
        }
        public async Task<Test?> GetTestByIdAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var testQuery = "SELECT * FROM Tests WHERE Id = @TestId";
                var test = await connection.QuerySingleOrDefaultAsync<Test>(testQuery, new { TestId = testId });

                if (test != null)
                {
                    var tagsQuery = "SELECT Tag FROM TestTags WHERE TestId = @TestId";
                    var tags = await connection.QueryAsync<int>(tagsQuery, new { TestId = testId });
                    test.Tags = new HashSet<TestTag>(tags.Select(tag => (TestTag)tag));

                    var commentsQuery = "SELECT * FROM Comments WHERE TestId = @TestId";
                    var comments = await connection.QueryAsync<Comment>(commentsQuery, new { TestId = testId });
                    test.Comments = comments.ToList();

                    var takingsQuery = "SELECT * FROM TestsTakings WHERE TestId = @TestId";
                    var takings = await connection.QueryAsync<TestsTaking>(takingsQuery, new { TestId = testId });
                    test.Takings = takings.ToList();

                    var ratingsQuery = "SELECT * FROM TestsRatings WHERE TestId = @TestId";
                    var ratings = await connection.QueryAsync<TestsRating>(ratingsQuery, new { TestId = testId });
                    test.Ratings = ratings.ToList();

                    var questionsQuery = "SELECT * FROM Questions WHERE TestId = @TestId";
                    var questions = await connection.QueryAsync<Question>(questionsQuery, new { TestId = testId });
                    test.Questions = questions.ToList();

                    var resultsQuery = "SELECT * FROM Results WHERE TestId = @TestId";
                    var results = await connection.QueryAsync<Result>(resultsQuery, new { TestId = testId });
                    test.Results = results.ToList();
                }

                return test;
            }
        }
        public async Task<IEnumerable<TestMainInfo>> GetAllTestsMainInfoAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
SELECT 
    t.Id, 
    t.Name, 
    t.ImagePath,
    (SELECT COUNT(*) FROM Questions WHERE TestId = t.Id) AS QuestionsCount,
    (SELECT COUNT(*) FROM Comments WHERE TestId = t.Id) AS CommentsCount,
    (SELECT COUNT(*) FROM TestsTakings WHERE TestId = t.Id) AS TakingsCount,
    (SELECT AVG(Rating) FROM TestsRatings WHERE TestId = t.Id) AS AverageRating
FROM Tests t";

                var testMainInfos = await connection.QueryAsync<TestMainInfo>(query);

                foreach (var testMainInfo in testMainInfos)
                {
                    var tags = await GetTagsForTestAsync(testMainInfo.Id);
                    testMainInfo.Tags = tags.Select(tag => tag.ToString()).ToList();
                }
                return testMainInfos;
            }
        }


        public async Task<IEnumerable<Question>> GetQuestionsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Questions WHERE TestId = @TestId";
                return await connection.QueryAsync<Question>(query, new { TestId = testId });
            }
        }
        public async Task<IEnumerable<Result>> GetResultsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Results WHERE TestId = @TestId";
                return await connection.QueryAsync<Result>(query, new { TestId = testId });
            }
        }
        public async Task<IEnumerable<Comment>> GetCommentsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Comments WHERE TestId = @TestId";
                return await connection.QueryAsync<Comment>(query, new { TestId = testId });
            }
        }
        public async Task<IEnumerable<TestTag>> GetTagsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Tag FROM TestTags WHERE TestId = @TestId";
                return await connection.QueryAsync<TestTag>(query, new { TestId = testId });
            }
        }
        public async Task<IEnumerable<TestsTaking>> GetTestsTakingsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TestsTakings WHERE TestId = @TestId";
                return await connection.QueryAsync<TestsTaking>(query, new { TestId = testId });
            }
        }
        public async Task<IEnumerable<TestsRating>> GetTestsRatingsForTestAsync(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TestsRatings WHERE TestId = @TestId";
                return await connection.QueryAsync<TestsRating>(query, new { TestId = testId });
            }
        }
    }
}
