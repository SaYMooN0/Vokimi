using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.dtos.draft_tests
{
    public class DraftGenericTestDto
    {
        public DraftTestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public DraftTestMainInfoDto MainInfo { get; protected set; }
        public DateTime CreationDate { get; init; }
        public TestConclusionId? ConclusionId { get; protected set; }
        public int QuestionsCount { get;protected set; }
        public DraftGenericTestDto(DraftGenericTest test) {
            Id = test.Id;
            CreatorId = test.CreatorId;
            MainInfo = new DraftTestMainInfoDto(test.MainInfo);
            CreationDate = test.CreationDate;
            ConclusionId = test.Conclusion is null ? null : test.Conclusion.Id;
            QuestionsCount = test.Questions.Count;
        }

    }
}
