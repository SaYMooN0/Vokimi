using Vokimi.Models;
using Vokimi.Models.DataBaseClasses;
using Vokimi.Models.ViewModels.Account;

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
        public Task<bool> AnyUserWithSuchEmail(string email);
        public Task<UserProfileViewModel?> GetUserInfo(int id);
        public Task<MyAccountViewModel?> GetMyAccountInfo(int userId);
        public Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        public Task<Test?> GetTestByIdAsync(int testId);
        public Task<IEnumerable<TestMainInfo>> GetAllTestsMainInfoAsync();
        public Task<IEnumerable<Question>> GetQuestionsForTestAsync(int testId);
        public Task<IEnumerable<Result>> GetResultsForTestAsync(int testId);
        public Task<IEnumerable<Comment>> GetCommentsForTestAsync(int testId);
        public Task<IEnumerable<TestTag>> GetTagsForTestAsync(int testId);
        public Task<IEnumerable<TestsTaking>> GetTestsTakingsForTestAsync(int testId); 
        public Task<IEnumerable<TestsRating>> GetTestsRatingsForTestAsync(int testId);


    }
}
