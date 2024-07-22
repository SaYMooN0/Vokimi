using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.dtos.draft_tests
{
    public class DraftTestMainInfoDto
    {
        public DraftTestMainInfoId Id { get; init; }
        public string Name { get; init; }
        public string CoverImagePath { get; init; }
        public string? Description { get; init; }
        public Language Language { get; init; }
        public TestPrivacy Privacy { get; init; }
        public DraftTestMainInfoDto(DraftTestMainInfo info) {
            Id = info.Id;
            Name = info.Name;
            CoverImagePath = info.CoverImagePath;
            Description = info.Description;
            Language = info.Language;
            Privacy = info.Privacy;
        }
    }
}
