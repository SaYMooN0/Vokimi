namespace VokimiShared.src
{
    public static class ImgOperationsHelper
    {
        //folders
        public const string
            GeneralFolder = "general",
            //users
            ProfilePicturesFolder = "profile_pictures",
            //tests
            TestsFolder = "published_test",
            TestConclusionsFolder = "test_conclusions",
            //draft tests
            DraftTestCoversFolder = "draft_tests_covers",
            DraftTestQuestionsFolder = "draft_tests_questions",
            DraftTestAnswersFolder = "draft_tests_answers",
            DraftTestResultsFolder = "draft_tests_results"
            ;
        public const string
            TestCoverFileName="test_cover";
        public static string DefaultTestCoverImg => $"{GeneralFolder}/test_cover_default.webp";
        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/GetImage/{fileKey}";
        public static string ImageUrlWithVersion(string path) =>
            $"{ImgUrl(path)}?v={Guid.NewGuid()}";
    }
}
