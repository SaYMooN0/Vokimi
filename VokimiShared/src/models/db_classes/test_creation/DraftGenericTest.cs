using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftGenericTest : IDraftTest
    {
        public DraftTestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public virtual AppUser Creator { get; set; }
        public DraftTestMainInfoId MainInfoId { get; init; }
        public virtual DraftTestMainInfo MainInfo { get; set; }
        public DateTime CreationDate { get; init; }
        //public HashSet<TestTag> Tags { get; init; } = new();
        public TestConclusionId? ConclusionId { get; init; }
        public virtual TestConclusion? Conclusion { get; set; }

        //Questions 
        //results
        public static DraftGenericTest CreateNew(AppUserId creatorId, DraftTestMainInfoId mainInfoId) =>
            new() {
                Id = new(),
                CreatorId = creatorId,
                MainInfoId = mainInfoId,
                CreationDate = DateTime.UtcNow,
                ConclusionId = null
            };
    }
}
