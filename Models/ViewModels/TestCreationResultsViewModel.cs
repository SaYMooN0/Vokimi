using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels
{
    public class TestCreationResultsViewModel
    {
        public string? ErrorMessage { get; set; }
        public int MaxAvailablePoints { get; set; }
        public int MinAvailablePoints { get; set; }
        public List<ResultData> ResultItems { get; set; } = new();
        public TestCreationResultsViewModel(int maxAvailablePoints, int minAvailablePoints, List<Result> resultItems)
        {
            MaxAvailablePoints = maxAvailablePoints;
            MinAvailablePoints = minAvailablePoints;
            ResultItems = new();
            foreach(Result r in resultItems)
            {
                if(r!=null)
                    ResultItems.Add(new ResultData(r.Text, r.GapMin, r.GapMax, r.Description));
            }
        }
    }

    public class ResultData
    {
        public ResultData(string mainText, int from, int to, string? description)
        {
            MainText = mainText;
            From = from;
            To = to;
            Description = description;
        }

        public string MainText { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string? Description { get; set; } = "";
    }
}
