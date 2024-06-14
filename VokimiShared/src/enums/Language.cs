namespace VokimiShared.src.enums
{
    public enum Language
    {
        Eng,
        Rus,
        Spa,
        Ger,
        Fra,
        Unset
    }
    public static class LanguageExtensions
    {
        public static string LanguageName(this Language lang) => lang switch
        {
            Language.Eng => "English",
            Language.Rus => "Русский",
            _ => throw new NotImplementedException()
        };

    }
}
