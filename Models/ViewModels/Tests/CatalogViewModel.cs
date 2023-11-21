using System.Reflection.Emit;
using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.Tests
{
    public class CatalogViewModel
    {
        public List<TestMainInfo> Tests { get; set; } = new();
        public TestsFilter Filter { get; set; } = new();
        public SortType SortType { get; set; } = SortType.Date;
        public bool IsSortAscending { get; set; } = true;
        public string TopMessage { get; set; } = "";
        public void FilterTests()
        {
            FilterByQuestionsCount();
            FilterByCommentsCount();
            FilterByPassingsCount();
            FilterByAverageRating();
            FilterByTags();
            FilterByLanguages();
            FilterByAge();

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

        public void FilterByTags()
        {
            if (Filter.ChosenTags is null || Filter.ChosenTags.Count() < 1)
                return;

        }
        private void FilterByLanguages()
        {
            if (Filter.ChosenLanguages is null || Filter.ChosenLanguages.Count() < 1)
                return;
            Tests = Tests.Where(test =>
                Filter.ChosenLanguages.Any(language => language == test.Language
                    )).ToList();
        }
        private void FilterByAge()
        {
            if (Filter.ChosenAges is null || Filter.ChosenAges.Count() < 1)
                return;
            Tests = Tests.Where(test =>
                Filter.ChosenAges.Any(age => age == test.AgeRestriction
                    )).ToList();
        }
        public void FilterByInEnumerable(IEnumerable<int> items)
        {
            Tests = Tests.Where(test => items.Any(i => i == test.Id)).ToList();
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
        public List<Language> ChosenLanguages { get; set; } = new();
        public List<AgeRestriction> ChosenAges { get; set; } = new();
        public bool OnlyPinned { get; set; } = false;
    }
}
