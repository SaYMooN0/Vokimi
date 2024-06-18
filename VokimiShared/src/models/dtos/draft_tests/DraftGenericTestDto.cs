using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.dtos.draft_tests;
using VokimiShared.src.models.dtos.tests;

namespace VokimiShared.src.models.dtos
{
    public class DraftGenericTestDto
    {
        public DraftTestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public DraftTestMainInfoDto MainInfo { get; protected set; }
        public DateTime CreationDate { get; init; }
        public TestConclusionDto? Conclusion { get; protected set; }
        public TestTemplate Template { get; protected set; }
        public DraftGenericTestDto(DraftGenericTest test) {
            Id = test.Id;
            CreatorId = test.CreatorId;
            MainInfo = new DraftTestMainInfoDto(test.MainInfo);
            CreationDate = test.CreationDate;
            Conclusion = test.Conclusion is null ? null : new TestConclusionDto(test.Conclusion);
            Template = test.Template;
        }

    }
}
