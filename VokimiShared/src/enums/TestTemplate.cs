namespace VokimiShared.src.enums
{
    public enum TestTemplate
    {
        Generic,
        Knowledge
    }
    public static class TestTemplateExtensions
    {
        public static string[] Features(this TestTemplate template) =>
            template switch {
                TestTemplate.Generic => [
                    "Completely customize your test the way you want it",
                    "No restrictions or necessities (almost)"],
                TestTemplate.Knowledge => [
                    "See how well test takers know the subject",
                    "Specially selected types of questions and the method of evaluating answers"],
                _ => throw new NotImplementedException()
            };
        public static string ShortId(this TestTemplate template) =>
            template switch {
                TestTemplate.Generic => "gen",
                TestTemplate.Knowledge => "knwlg",
                _ => throw new NotImplementedException()
            };
    }
}
