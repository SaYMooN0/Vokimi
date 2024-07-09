namespace VokimiShared.src
{
    public static class ImgOperationsHelper
    {
        public const string
            ProfilePicturesFolder = "profile_pictures",
            TestCoversFolder = "test_covers",
            GeneralFolder = "general",
            DraftTestAnswersFolder = "draft_tests_answers",
            DraftTestCoversFolder = "draft_tests_covers",
            DraftTestConclusionsFolder = "draft_tests_conclusions";
        public static string DefaultTestCoverImg => $"{GeneralFolder}/test_cover_default.webp";
        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/GetImage/{fileKey}";
        public static string ImageUrlWithVersion(string path) =>
            $"{ImgUrl(path)}?v={Guid.NewGuid()}";
    }
}
