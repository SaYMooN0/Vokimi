using VokimiShared.src.enums;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftTestMainInfo
    {
        public DraftTestMainInfoId Id { get; init; }
        public string Name { get; private set; }
        public string CoverImagePath { get; private set; }
        public string? Description { get; private set; }
        public Language Language { get; private set; }
        public TestPrivacy Privacy { get; private set; }
        public static DraftTestMainInfo CreateNewFromName(string name) =>
            new() {
                Id = new(),
                Name = name,
                CoverImagePath = ImgOperationsHelper.DefaultTestCoverImg,
                Description = null,
                Language = Language.Other,
                Privacy = TestPrivacy.Anyone
            };

        public void UpdateCoverImage(string path) => CoverImagePath = path;
        public void Update(string name, string? description, Language language, TestPrivacy privacy) {
            Name = name;
            Description = description;
            Language = language;
            Privacy = privacy;
        }
    }

}
