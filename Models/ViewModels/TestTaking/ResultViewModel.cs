using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.TestTaking
{
    public class ResultViewModel
    {
        public int TestId { get; set; } 
        public string TestName { get; set; }
        public Result ReceivedResult { get; set; }  
        public List<Result> AllResults { get; set; }
        public Dictionary<int, int> ResultsFrequency { get; set; }
        public ResultViewModel(int testId, string testName, Result receivedResult, List<Result> allResults)
        {
            TestId = testId;
            TestName = testName;
            ReceivedResult = receivedResult;
            AllResults = allResults;
        }
    }
}
