using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.Tests
{
    public class CatalogViewModel
    {
        public List<TestMainInfo> Tests { get; set; } = new();
        public TestsFilter Filter { get; set; } = new();
        public SortType SortType { get; set; } = SortType.Date;
        public bool IsSortAscending { get; set;  } = true;
        public void FilterTests()
        {
            throw new NotImplementedException();
        }
    }
    public class TestsFilter
    {
        public int? MinQuestionsCount { get; set; }
        public int? MaxQuestionsCount { get; set; }
        public int? MinCommentsCount { get; set; }
        public int? MaxCommentsCount { get; set; }
        public int? MinPassingsCount { get; set; }
        public int? MaxPassingsCount { get; set; }
        public int? MinAverageRating { get; set; }
        public int? MaxAverageRating { get; set; }
        public List<string> ChosenTags { get; set; } = new();
    }
}
