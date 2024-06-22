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
        public string Privacy { get; set; }
        public List<QuestionBriefInfo> Questions { get; set; } = [];
        public static TestCreationOverviewViewModel FromTestDto(DraftGenericTestDto dto) =>
            new() {
                Name = dto.MainInfo.Name,
                Description = string.IsNullOrEmpty(dto.MainInfo.Description) ? "(None)" : dto.MainInfo.Description,
                CoverPath = dto.MainInfo.CoverImagePath,
                Language = dto.MainInfo.Language.FullName(),
                Privacy = dto.MainInfo.Privacy.ToString(),
                Questions=new()
            };

    }
    public record class QuestionBriefInfo(
        string Text,
        bool IsMultiAnswer,
        int AnswersCount
        );
}
