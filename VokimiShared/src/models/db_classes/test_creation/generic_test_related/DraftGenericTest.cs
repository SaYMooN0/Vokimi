using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_creation.generic_test_related
{
    public class DraftGenericTest : BaseDraftTest
    {

        public virtual ICollection<DraftGenericTestQuestion> Questions { get; private set; } = [];
        public DraftGenericTest() {
            Template = TestTemplate.Generic;
        }
        public static DraftGenericTest CreateNew(AppUserId creatorId, DraftTestMainInfoId mainInfoId, TestStylesSheetId stylesSheetId) =>
            new() {
                Id = new(),
                CreatorId = creatorId,
                MainInfoId = mainInfoId,
                CreationDate = DateTime.UtcNow,
                ConclusionId = null,
                StylesSheetId = stylesSheetId
            };
    }
}
