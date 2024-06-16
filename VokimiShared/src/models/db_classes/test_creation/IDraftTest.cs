
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public interface IDraftTest
    {
        public DraftTestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public AppUser Creator { get; protected set; }
        public DraftTestMainInfoId MainInfoId { get; init; }
        public DraftTestMainInfo MainInfo { get; protected set; }
        public DateTime CreationDate { get; init; }
        //public HashSet<TestTag> Tags { get; init; }
        public TestConclusionId? ConclusionId { get; init; }
        public TestConclusion? Conclusion { get; protected set; }
    }
}
