using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels.Account;
using Vokimi.Models.ViewModels.Tests;

namespace VokimiServices
{
    public interface IDataBase
    {
        public Task<int> AddNewUser(User user);
        public Task<int> AddNewTest(Test test);
        public Task<int> CreateNewTest(TestCreationData testCreationData, int authorId);
        public Task<int> AddNewResult(Result result, int testId);
        public Task<IEnumerable<int>> AddNewResults(IEnumerable<Result> results, int testId);
        public Task<int> AddNewQuestion(Question question, int testId);
        public Task<IEnumerable<int>> AddNewQuestions(IEnumerable<Question> question, int testId);
        public Task<int> AddTagForTest(TestTag tag, int testId);
        public Task<IEnumerable<int>> AddTagsForTest(IEnumerable<TestTag> tags, int testId);
        public Task<int> AddCommentForTest(int testId, string Text, int userId);
        public Task<bool> AnyUserWithSuchEmail(string email);
        public Task<UserProfileViewModel?> GetUserInfo(int id);
        public Task<MyAccountViewModel?> GetMyAccountInfo(int userId);
        public Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        public Task<Test?> GetTestByIdAsync(int testId);
        public Task<IEnumerable<TestMainInfo>> GetAllTestsMainInfoAsync(int? userId);
        public Task<IEnumerable<Question>> GetQuestionsForTestAsync(int testId);
        public Task<IEnumerable<Result>> GetResultsForTestAsync(int testId);
        public Task<IEnumerable<Comment>> GetCommentsForTestAsync(int testId);
        public Task<IEnumerable<TestTag>> GetTagsForTestAsync(int testId);
        public Task<IEnumerable<TestsTaking>> GetTestsTakingsForTestAsync(int testId);  
        public Task<IEnumerable<TestsRating>> GetTestsRatingsForTestAsync(int testId);
        public Task<bool> IsTestPinnedByUserAsync(int testId, int userId);
        public Task<string> GetUserNameById(int userId);
        public Task<IEnumerable<CommentInfo>> GetCommentsInfoForTest(int testId);
        public Task RateTestAsync(int testId, short rating, int userId);
        public Task<bool> PinUnpinTestForUser(int testId,int userId);
        public Task<IEnumerable<int>> GetPinnedTestsForUser(int userId);


    }
}
