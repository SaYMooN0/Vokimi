using Vokimi.Components.Pages.MyTests.tabs.creation_tab_components.test_templates.template_icons;
using Vokimi.Components.Pages.TestTaking.ArrowButtonIcons;
using VokimiShared.src.enums;

namespace Vokimi.Components
{
    public static class ComponentsHelper
    {

        public static Type TemplateIconComponent(this TestTemplate template) =>
            template switch {
                TestTemplate.Generic => typeof(TestTemplateIconGeneric),
                TestTemplate.Knowledge => typeof(TestTemplateIconKnowledge),
                _ => throw new ArgumentOutOfRangeException(nameof(template), template, null)
            };
        public static Type ArrowIconComponent(this ArrowIconType type, bool isRight) =>
            (type, isRight) switch {
                (ArrowIconType.Default, true) => typeof(ArrowDefaultRight),
                (ArrowIconType.Default, false) => typeof(ArrowDefaultLeft),
                (ArrowIconType.DefaultCircle, true) => typeof(ArrowDefaultCircleRight),
                (ArrowIconType.DefaultCircle, false) => typeof(ArrowDefaultCircleLeft),
                (ArrowIconType.Double, true) => typeof(ArrowDoubleRight),
                (ArrowIconType.Double, false) => typeof(ArrowDoubleLeft),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

    }
}
