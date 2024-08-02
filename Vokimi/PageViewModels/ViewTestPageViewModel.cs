using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.PageViewModels
{
    public record class ViewTestPageViewModel(
        string Name,
        string ImagePath,
        TestTemplate Template,
        AppUserId CreatorId
        ) {
        public static ViewTestPageViewModel FromTest(BaseTest test)=>
            new(test.Name, test.Cover, test.Template, test.CreatorId);

    }
}
