namespace VokimiShared.src
{
    public static class ImgOperationsHelper
    {
        public const string
            GeneralFolder = "general",
            //users
            ProfilePicturesFolder = "profile_pictures",
            //tests
            TestCoversFolder = "test_covers",
            //draft tests
            DraftTestCoversFolder = "draft_tests_covers",
            DraftTestQuestionsFolder = "draft_tests_questions",
            DraftTestAnswersFolder = "draft_tests_answers",
            DraftTestConclusionsFolder = "draft_tests_conclusions",
            DraftTestResultsFolder = "draft_tests_results"
            ;
        public static string DefaultTestCoverImg => $"{GeneralFolder}/test_cover_default.webp";
        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/GetImage/{fileKey}";
        public static string ImageUrlWithVersion(string path) =>
            $"{ImgUrl(path)}?v={Guid.NewGuid()}";
    }
}
