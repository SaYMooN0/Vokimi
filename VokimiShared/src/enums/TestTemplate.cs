
namespace VokimiShared.src.enums
{
    public enum TestTemplate
    {
        None,
        Knowledge
    }
    public static class TestTemplateExtensions
    {
        public static string[] Features(this TestTemplate template) =>
            template switch {
                TestTemplate.None => [
                    "completely customize your test the way you want it",
                    "no restrictions or necessities (almost)"],
                TestTemplate.Knowledge => [
                    "see how well test takers know the subject",
                    "specially selected types of questions and the method of evaluating answers"],
                _ => throw new NotImplementedException()
            };
    }
}
