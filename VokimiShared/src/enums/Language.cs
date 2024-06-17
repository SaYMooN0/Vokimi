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
        public static string FullName(this Language lang) => lang switch
        {
            Language.Eng => "English",
            Language.Rus => "Русский",
            Language.Spa => "Español",
            Language.Ger => "Deutsch",
            Language.Fra => "Français",
            Language.Unset => "Unset",
            _ => throw new NotImplementedException()
        };

    }
}
