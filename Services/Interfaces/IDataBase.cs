using System.Data.SqlClient;
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
        public Task<bool> AnyUserWithSuchEmail(string email);
        public Task<UserProfileViewModel?> GetUserInfo(int id);
        public Task<MyAccountViewModel?> GetMyAccountInfo(int userId);
        public Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        public Task<Test?> GetTestByIdAsync(int testId);
        public Task<List<TestMainInfo>> GetAllTestsMainInfoAsync();


    }
}
