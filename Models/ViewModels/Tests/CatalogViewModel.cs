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
            FilterByQuestionsCount();
            FilterByCommentsCount();
            FilterByPassingsCount();
            FilterByAverageRating();
            FilterByTags();
            FilterByLanguages();

        }
        private void FilterByQuestionsCount()
        {
            if (Filter.MinQuestionsCount is not null)
                Tests = Tests.Where(test => test.QuestionsCount >= Filter.MinQuestionsCount.Value).ToList();
            if (Filter.MaxQuestionsCount is not null)
                Tests = Tests.Where(test => test.QuestionsCount <= Filter.MaxQuestionsCount.Value).ToList();
        }
        private void FilterByCommentsCount()
        {
            if (Filter.MinCommentsCount is not null)
                Tests = Tests.Where(test => test.CommentsCount >= Filter.MinCommentsCount.Value).ToList();
            if (Filter.MaxCommentsCount is not null)
                Tests = Tests.Where(test => test.CommentsCount <= Filter.MaxCommentsCount.Value).ToList();
        }
        private void FilterByPassingsCount()
        {
            if (Filter.MinPassingsCount is not null)
                Tests = Tests.Where(test => test.TakingsCount >= Filter.MinPassingsCount.Value).ToList();
            if (Filter.MaxPassingsCount is not null)
                Tests = Tests.Where(test => test.TakingsCount <= Filter.MaxPassingsCount.Value).ToList();
        }

        private void FilterByAverageRating()
        {
            if (Filter.MinAverageRating is not null)
                Tests = Tests.Where(test => test.AverageRating >= Filter.MinAverageRating.Value).ToList();
            if (Filter.MaxAverageRating is not null)
                Tests = Tests.Where(test => test.AverageRating <= Filter.MaxAverageRating.Value).ToList();
        }

        public void FilterByTags() {
        }
        private void FilterByLanguages() {
        
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
        public List<string> ChosenLanguages { get; set; }=new();
        public bool OnlyPinned { get; set; } = false;
    }
}
