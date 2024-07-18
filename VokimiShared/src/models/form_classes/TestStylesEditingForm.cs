
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test;

namespace VokimiShared.src.models.form_classes
{
    public class TestStylesEditingForm
    {
        public string AccentColor { get; set; }
        public ArrowIconType ArrowsType { get; set; }
        public static TestStylesEditingForm Default =>
            new() {
                AccentColor = TestCreationConsts.DefaultAccentColor,
                ArrowsType = ArrowIconType.Default,
            };
        public static TestStylesEditingForm FromTestStylesSheet(TestStylesSheet styles) =>
            new() {
                AccentColor = styles.AccentColor,
                ArrowsType = styles.ArrowsType
            };

    }
}
