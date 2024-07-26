using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test
{
    public class TestStylesSheet
    {
        public TestStylesSheetId Id { get; set; }
        public string AccentColor { get; private set; }
        public ArrowIconType ArrowsType { get; private set; }
        public void Update(string newAccentColor, ArrowIconType newArrowsType) {
            AccentColor = newAccentColor;
            ArrowsType = newArrowsType;
        }
        public static TestStylesSheet CreateNew() => new() {
            Id = new(),
            AccentColor = BaseTestCreationConsts.DefaultAccentColor,
            ArrowsType = ArrowIconType.Default
        };
    }
}
