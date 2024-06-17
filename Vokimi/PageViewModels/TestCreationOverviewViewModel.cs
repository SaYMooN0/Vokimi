using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation;

namespace Vokimi.PageViewModels
{
    public class TestCreationOverviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Language { get; set; }
        public string Privace { get; set; }
        public List<QuestionOverviewViewModel> Questions { get; set; } = [];
        public static TestCreationOverviewViewModel FromTest(BaseDraftTest draftTest) =>
            new() {
                Name = draftTest.MainInfo.Name,
                Description = string.IsNullOrEmpty(draftTest.MainInfo.Description) ? "(None)" : draftTest.MainInfo.Description,
                ImagePath = draftTest.MainInfo.CoverImagePath,
                Language = draftTest.MainInfo.Language.FullName(),
                Privace = draftTest.MainInfo.Privacy.ToString()
            };

    }
    public record class QuestionOverviewViewModel(
        string Text,
        bool IsMultiAnswer,
        int AnswersCount,
        int MinPossiblePoints,
        int MaxPossiblePoints
        );
}
