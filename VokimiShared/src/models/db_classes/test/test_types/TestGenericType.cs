using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.db_classes.test.test_types
{
    public class TestGenericType : BaseTest
    {
        public override TestTemplate Template => TestTemplate.Generic;

        public virtual ICollection<GenericTestQuestion> Questions { get; init; } = new List<GenericTestQuestion>();
        public virtual ICollection<TestResult> PossibleResults { get; init; } = new List<TestResult>();

        public static TestGenericType CreateNewFromDraft(DraftGenericTest draft,string coverPath) => new() {
            Id=new(),
            CreatorId=draft.CreatorId,
            Name=draft.MainInfo.Name,
            Cover= coverPath,
            Description=draft.MainInfo.Description,
            Language=draft.MainInfo.Language,
            Privacy=draft.MainInfo.Privacy,
            CreationDate=draft.CreationDate,
            PublicationDate=DateTime.UtcNow,
            ConclusionId=draft.ConclusionId,
            StylesSheetId=draft.StylesSheetId

        };
    }
}
