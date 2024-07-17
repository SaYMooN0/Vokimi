
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;

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
    }
}
