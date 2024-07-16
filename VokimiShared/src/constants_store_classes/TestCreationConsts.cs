namespace VokimiShared.src.constants_store_classes
{
    public class TestCreationConsts
    {
        public const int
            MaxResultsCountForAnswer = 5,
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
