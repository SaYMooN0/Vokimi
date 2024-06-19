using VokimiShared.src.enums;
using VokimiShared.src.models.dtos;

namespace Vokimi.PageViewModels
{
    public class TestCreationOverviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
        public string Language { get; set; }
        public string Privace { get; set; }
        public List<QuestionOverviewViewModel> Questions { get; set; } = [];
        public static TestCreationOverviewViewModel FromTestDto(DraftGenericTestDto dto) =>
            new() {
                Name = dto.MainInfo.Name,
                Description = string.IsNullOrEmpty(dto.MainInfo.Description) ? "(None)" : dto.MainInfo.Description,
                CoverPath = dto.MainInfo.CoverImagePath,
                Language = dto.MainInfo.Language.FullName(),
                Privace = dto.MainInfo.Privacy.ToString(),
                Questions=new()
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
