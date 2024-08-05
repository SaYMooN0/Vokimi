using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.PageViewModels
{
    public record class ViewTestPageViewModel(
        string Name,
        string CoverPath,
        TestTemplate Template,
        Language TestLang,
        string? Description,
        string[] Tags,
        AppUserId CreatorId,
        string CreatorUsername
        )
    {
        public static ViewTestPageViewModel Empty => new(
            string.Empty,
            string.Empty,
            TestTemplate.Generic,
            Language.Other,
            null,
            [],
            new(),
            string.Empty
        );
        public static ViewTestPageViewModel FromTest(BaseTest test) => new(
            test.Name,
            test.Cover,
            test.Template,
            test.Language,
            test.Description,
            test.Tags.Select(t => t.Value).ToArray(),
            test.CreatorId,
            test.Creator.Username
        );

    }
}
