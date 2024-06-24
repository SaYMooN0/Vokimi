using VokimiShared.src.models.db_classes;
using VokimiShared.src;

namespace Vokimi.PageViewModels
{
    public class TestCoverAreaContentViewModel
    {
        public string CoverPath { get; private set; } = ImgOperationsHelper.DefaultTestCoverImg;
        public DraftTestMainInfoId MainInfoId { get; init; }
        public DraftTestId TestId { get; init; }
        public void UpdateCoverPath(string path) => CoverPath = path;
        public TestCoverAreaContentViewModel(string coverPath, DraftTestMainInfoId mainInfoId, DraftTestId testId) {
            CoverPath = coverPath;
            MainInfoId = mainInfoId;
            TestId = testId;
        }
    }
}
