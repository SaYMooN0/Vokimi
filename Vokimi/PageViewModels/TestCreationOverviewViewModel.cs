using VokimiShared.src.enums;
using VokimiShared.src.models.dtos.draft_tests;

namespace Vokimi.PageViewModels
{
    public class TestCreationOverviewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverPath { get; set; }
        public string Language { get; set; }
        public string Privacy { get; set; }
        public List<QuestionBriefInfoDto> Questions { get; set; } = [];
        public static TestCreationOverviewViewModel FromTestDto(DraftGenericTestDto dto) =>
            new() {
                Name = dto.MainInfo.Name,
                Description = string.IsNullOrEmpty(dto.MainInfo.Description) ? "(None)" : dto.MainInfo.Description,
                CoverPath = dto.MainInfo.CoverImagePath,
                Language = dto.MainInfo.Language.FullName(),
                Privacy = dto.MainInfo.Privacy.ToString(),
                Questions = dto.Questions
            };

    }
}
