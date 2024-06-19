using VokimiShared.src.enums;

namespace VokimiShared.src.models.db_classes.tests
{
    public class DraftTestMainInfo
    {
        public DraftTestMainInfoId Id { get; init; }
        public string Name { get; init; }
        public string CoverImagePath { get; init; }
        public string? Description { get; init; }
        public Language Language { get; init; }
        public TestPrivacy Privacy { get; init; }
        public static DraftTestMainInfo CreateNewFromName(string name) =>
            new() {
                Id=new(),
                Name = name,
                CoverImagePath = ImgOperationsHelper.DefaultTestCoverImg,
                Description = null,
                Language = Language.Unset,
                Privacy = TestPrivacy.Anyone
            };

    }

}
