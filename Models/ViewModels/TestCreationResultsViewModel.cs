namespace Vokimi.Models.ViewModels
{
    public class TestCreationResultsViewModel
    {
        public string? ErrorMessage { get; set; }
        public int MaxAvailablePoints { get; set; }
        public int MinAvailablePoints { get; set; }
        public List<ResultData> ResultItems { get; set; } = new();
    }
    public class ResultData
    {
        public string MainText { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Description { get; set; } = "";
    }
}
