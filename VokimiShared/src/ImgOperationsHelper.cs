namespace VokimiShared.src
{
    public static class ImgOperationsHelper
    {
        public const string
            ProfilePicturesFolder = "profile_pictures",
            TestCoversFolder = "test_covers",
            GeneralFolder = "general",
            DraftTestCoversFolder = "draft_tests_covers";
        public static string DefaultTestCoverImg => $"{GeneralFolder}/test_cover_default.webp";
        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/GetImage/{fileKey}";
    }
}
