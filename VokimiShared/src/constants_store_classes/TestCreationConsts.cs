
using System.Text.RegularExpressions;

namespace VokimiShared.src.constants_store_classes
{
    public class TestCreationConsts
    {
        public const int
            MaxResultsCountForAnswer = 5,
            ResultMaxCharacters = 12,
            ResultMinCharacters = 3;
        public const int MaxImageSizeInBytes = 3145728;
        public static int MaxImageSizeInMB => MaxImageSizeInBytes / 1048576;

        public const int
            ConclusionMaxCharsInText = 255,
            ConclusionMaxAccompanyingFeedbackTextChars = 255,
            ConclusionMaxFeedbackCharsCount= 255;
    }
}
