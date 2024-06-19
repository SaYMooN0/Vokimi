using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace Vokimi.PageViewModels
{
    public class DraftTestViewModel
    {
        public DraftTestId Id { get; init; }
        public bool IsReady { get; init; }
        public string ImagePath { get; init; }
        public string Name { get; set; }
        public string OverviewLink { get; init; }
        public TestTemplate Template { get; init; }
        public static DraftTestViewModel FromTest(BaseDraftTest test) =>
            new() {
                Id=test.Id,
                IsReady=false,
                ImagePath=test.MainInfo.CoverImagePath,
                Name=test.MainInfo.Name,
                OverviewLink= $"/newtest/{test.Id}",
                Template=test.Template
            };
    }
}
