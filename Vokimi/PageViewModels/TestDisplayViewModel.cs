using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.PageViewModels
{
    public class TestDisplayViewModel
    {
        public string ImagePath { get; init; }
        public string Name { get; set; }
        public string OverviewLink { get; init; }
        public TestTemplate Template { get; init; }
        public static TestDisplayViewModel FromDraftTest(BaseDraftTest test) =>
            new() {
                ImagePath=test.MainInfo.CoverImagePath,
                Name=test.MainInfo.Name,
                OverviewLink= $"/newtest/{test.Id}",
                Template=test.Template
            };
        public static TestDisplayViewModel FromPublishedTest (BaseTest test) =>
            new() {
                ImagePath = test.Cover,
                Name = test.Name,
                OverviewLink = $"/test-managing/{test.Id}",
                Template = test.Template
            };
    }
}
