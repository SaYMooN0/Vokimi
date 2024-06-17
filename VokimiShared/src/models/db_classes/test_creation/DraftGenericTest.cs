using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftGenericTest : BaseDraftTest
    {
        //public HashSet<TestTag> Tags { get; init; } = new();

        //Questions 
        //results
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
