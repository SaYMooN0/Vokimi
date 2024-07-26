
namespace VokimiShared.src.constants_store_classes
{
    public static class BaseTestCreationConsts
    {
        public const int
            MinTestNameLength = 12,
            MaxTestNameLength = 255,
            MaxTestDescriptionLength = 255;
        public const int MaxTagsForTestCount = 12;
        public const int
            MaxResultsForTestCount = 30,
            ResultMaxTextLength = 500,
            ResultMinTextLength = 8,
            ResultIdMaxCharacters = 12,
            ResultIdMinCharacters = 5;
        public const int MaxImageSizeInBytes = 3145728;
        public static int MaxImageSizeInMB => MaxImageSizeInBytes / 1048576;

        public const int
            ConclusionMaxCharsInText = 255,
            ConclusionMaxAccompanyingFeedbackTextChars = 255,
            ConclusionMaxFeedbackCharsCount = 255;

        public const string DefaultAccentColor = "#796cfa";
    }
}
