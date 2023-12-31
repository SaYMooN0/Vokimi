﻿using Dapper;
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
        public async Task<User?> GetUserById(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var userQuery = "SELECT * FROM Users WHERE Id = @UserId";
                var user = await connection.QueryFirstOrDefaultAsync<User>(userQuery, new { UserId = userId });

                if (user == null)
                    return null;
                var createdTestsQuery = "SELECT Id FROM Tests WHERE AuthorId = @UserId";
                var createdTestsId = (await connection.QueryAsync<int>(createdTestsQuery, new { UserId = userId })).ToList();
                var ratedTestsQuery = "SELECT TestId FROM TestsRatings WHERE UserId = @UserId";
                var ratedTestsId = (await connection.QueryAsync<int>(ratedTestsQuery, new { UserId = userId })).ToList();
                var pinnedTestsQuery = "SELECT TestId FROM PinnedTests WHERE UserId = @UserId";
                var pinnedTestsId = (await connection.QueryAsync<int>(pinnedTestsQuery, new { UserId = userId })).ToList();
                var takenTestsQuery = "SELECT TestId FROM TestsTakings WHERE UserId = @UserId";
                var takenTestsId = (await connection.QueryAsync<int>(takenTestsQuery, new { UserId = userId })).ToList();

                var preferredLanguagesRaw = await connection.QueryAsync<int>("SELECT Language FROM UserPreferredLanguages WHERE UserId = @UserId", new { UserId = userId });
                var preferredLanguages = new HashSet<Language>(preferredLanguagesRaw.Select(lang => (Language)lang));

                return new User(
                    user.Name,
                    user.Email,
                    user.Password,
                    user.BirthDate,
                    user.IsBanned,
                    user.Role,
                    user.RegistrationDate,
                    user.Status,
                    commentsId: new List<int>(),
                    takenTestsId: takenTestsId,
                    ratedTestsId: ratedTestsId,
                    createdTestsId: createdTestsId,
                    pinnedTestsId: pinnedTestsId,
                    preferredLanguages: preferredLanguages
                );
            }
        }


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
                User? user = await GetUserById(userId);

                if (user is null)
                    return null;

                UserProfileViewModel viewModel = new(userId, user.Name, user.Status);
                viewModel.LastTakenTest = (await GetTestsMainInfoListAsync(user.TakenTestsId, userId)).FirstOrDefault();
                viewModel.CreatedTests = await GetTestsMainInfoListAsync(user.CreatedTestsId, userId);
                return viewModel;
            }
        }
        private async Task<List<TestMainInfo>> GetTestsMainInfoListAsync(List<int> testIds, int? userId)
        {
            var tasks = testIds.Select(id => GetTestMainInfoAsync(id, userId));
            var testMainInfos = await Task.WhenAll(tasks);
            return testMainInfos.ToList();
        }
        public async Task<MyAccountViewModel?> GetMyAccountInfo(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                User? user = await GetUserById(userId);

                if (user is null)
                    return null;

                MyAccountViewModel viewModel = new(userId, user.Name, user.Email, user.Status);

                viewModel.TakenTests = await GetTestsMainInfoListAsync(user.TakenTestsId, userId);
                viewModel.LastTakenTest = viewModel.TakenTests.FirstOrDefault();
                viewModel.CreatedTests = await GetTestsMainInfoListAsync(user.CreatedTestsId, userId);
                viewModel.RatedTests = await GetTestsMainInfoListAsync(user.RatedTestsId, userId);
                viewModel.PinnedTests = await GetTestsMainInfoListAsync(user.PinnedTestsId, userId);

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
        public async Task<TestMainInfo> GetTestMainInfoAsync(int testId, int? userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
SELECT 
    t.Id, 
    t.Name, 
    t.ImagePath,
    t.Language,
    t.AgeRestriction,
    (SELECT COUNT(*) FROM Questions WHERE TestId = t.Id) AS QuestionsCount,
    (SELECT COUNT(*) FROM Comments WHERE TestId = t.Id) AS CommentsCount,
    (SELECT COUNT(*) FROM TestsTakings WHERE TestId = t.Id) AS TakingsCount,
    (SELECT AVG(Rating) FROM TestsRatings WHERE TestId = t.Id) AS AverageRating
FROM Tests t
WHERE t.Id = @TestId";

                var testMainInfo = await connection.QuerySingleOrDefaultAsync<TestMainInfo>(query, new { TestId = testId });

                if (testMainInfo != null)
                {
                    var tags = await GetTagsForTestAsync(testMainInfo.Id);
                    testMainInfo.Tags = tags.Select(tag => tag.ToString()).ToList();
                    testMainInfo.IsPinned = userId.HasValue && await IsTestPinnedByUserAsync(testMainInfo.Id, userId.Value);
                }

                return testMainInfo;
            }
        }
        public async Task<IEnumerable<TestMainInfo>> GetAllTestsMainInfoAsync(int? userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var testIds = await connection.QueryAsync<int>("SELECT Id FROM Tests");

                var tasks = testIds.Select(testId => GetTestMainInfoAsync(testId, userId));
                var testMainInfos = await Task.WhenAll(tasks);

                return testMainInfos;
            }
        }

        public async Task<bool> IsTestPinnedByUserAsync(int testId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(1) FROM PinnedTests WHERE TestId = @TestId AND UserId = @UserId";
                var count = await connection.ExecuteScalarAsync<int>(query, new { TestId = testId, UserId = userId });
                return count > 0;
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

        public async Task<int> AddCommentForTest(int testId, string text, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO Comments (UserId, TestId, Text, LeavingDate) 
VALUES (@UserId, @TestId, @Text, @LeavingDate);

SELECT CAST(SCOPE_IDENTITY() as int);";

                var parameters = new
                {
                    UserId = userId,
                    TestId = testId,
                    Text = text,
                    LeavingDate = DateTime.Now
                };

                connection.Open();
                return await connection.QuerySingleAsync<int>(query, parameters);
            }
        }
        public async Task<string> GetUserNameById(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Name FROM Users WHERE Id = @UserId";

                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<string>(query, new { UserId = userId });
            }
        }
        public async Task<IEnumerable<CommentInfo>> GetCommentsInfoForTest(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
SELECT u.Name AS Author, c.Text, c.LeavingDate 
FROM Comments c
JOIN Users u ON c.UserId = u.Id
WHERE c.TestId = @TestId
ORDER BY c.LeavingDate DESC";

                connection.Open();
                return await connection.QueryAsync<CommentInfo>(query, new { TestId = testId });
            }
        }

        public async Task RateTestAsync(int testId, short rating, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
IF EXISTS (SELECT 1 FROM TestsRatings WHERE UserId = @UserId AND TestId = @TestId)
    UPDATE TestsRatings SET Rating = @Rating WHERE UserId = @UserId AND TestId = @TestId;
ELSE
    INSERT INTO TestsRatings (UserId, TestId, Rating) VALUES (@UserId, @TestId, @Rating);";

                connection.Open();
                await connection.ExecuteAsync(query, new { UserId = userId, TestId = testId, Rating = rating });
            }
        }
        public async Task<bool> PinUnpinTestForUser(int testId, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var checkQuery = "SELECT COUNT(1) FROM PinnedTests WHERE UserId = @UserId AND TestId = @TestId";
                var count = await connection.ExecuteScalarAsync<int>(checkQuery, new { UserId = userId, TestId = testId });

                if (count > 0)
                {
                    var unpinQuery = "DELETE FROM PinnedTests WHERE UserId = @UserId AND TestId = @TestId";
                    await connection.ExecuteAsync(unpinQuery, new { UserId = userId, TestId = testId });
                    return false;
                }
                else
                {
                    var pinQuery = "INSERT INTO PinnedTests (UserId, TestId) VALUES (@UserId, @TestId)";
                    await connection.ExecuteAsync(pinQuery, new { UserId = userId, TestId = testId });
                    return true;
                }
            }
        }
        public async Task<IEnumerable<int>> GetPinnedTestsForUser(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT TestId FROM PinnedTests WHERE UserId = @UserId";

                connection.Open();
                return await connection.QueryAsync<int>(query, new { UserId = userId });
            }
        }
        public async Task<int> AddTestTakingAsync(int userId, int testId, int resultId, DateTime takingDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
INSERT INTO TestsTakings (UserId, TestId, ResultId, TakingDate) 
VALUES (@UserId, @TestId, @ResultId, @TakingDate);

SELECT CAST(SCOPE_IDENTITY() as int);";

                connection.Open();
                return await connection.QuerySingleAsync<int>(query, new { UserId = userId, TestId = testId, ResultId = resultId, TakingDate = takingDate });
            }
        }
        public async Task<int?> GetResultIdByTestAndPoints(int testId, int points)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
SELECT Id FROM Results 
WHERE TestId = @TestId 
AND @Points >= GapMin 
AND @Points <= GapMax";

                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<int?>(query, new { TestId = testId, Points = points });
            }
        }
        public async Task<Dictionary<int, int>> GetResultsIdWithFrequencyForTest(int testId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
SELECT ResultId, COUNT(*) as Frequency 
FROM TestsTakings 
WHERE TestId = @TestId 
GROUP BY ResultId";

                connection.Open();
                var results = await connection.QueryAsync(query, new { TestId = testId });

                var resultDict = new Dictionary<int, int>();
                foreach (var result in results)
                {
                    resultDict.Add(result.ResultId, result.Frequency);
                }

                return resultDict;
            }
        }

    }
}
