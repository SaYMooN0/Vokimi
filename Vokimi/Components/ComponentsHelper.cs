using Vokimi.Components.Pages.MyTests.tabs.creation_tab_components.test_templates.template_icons;
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
    }
}
