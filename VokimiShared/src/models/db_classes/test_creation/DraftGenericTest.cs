using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftGenericTest : BaseDraftTest
    {

        public virtual ICollection<DraftTestQuestion> Questions { get; private set; } = new List<DraftTestQuestion>();


        //results
        //public HashSet<TestTag> Tags { get; init; } = new();

        public DraftGenericTest() {
            Template = TestTemplate.Generic;
        }
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
