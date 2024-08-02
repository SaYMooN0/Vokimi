using VokimiShared.src.models.db_entities_ids;

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
            TestAnswersFolder = "tests_answers",
            //draft tests
            DraftTestCoversFolder = "draft_tests_covers",
            DraftTestQuestionsFolder = "draft_tests_questions",
            DraftTestResultsFolder = "draft_tests_results";
        private const string
            ResultsFolderName = "results",
            QuestionsFolderName = "questions";
        public const string
            TestCoverFileName="test_cover";
        public static string DefaultTestCoverImg => $"{GeneralFolder}/test_cover_default.webp";
        public static string TestCoverImg(TestId testId) => $"{TestsFolder}/{testId}/{TestCoverFileName}";
        public static string TestResultsFolder(TestId testId) => $"{TestsFolder}/{testId}/{ResultsFolderName}/";
        public static string TestQuestionsFolder(TestId testId) => $"{TestsFolder}/{testId}/{QuestionsFolderName}/";

        public static string ImgUrl(string fileKey) =>
           $"vokimiimgs/GetImage/{fileKey}";
        public static string ImageUrlWithVersion(string path) =>
            $"{ImgUrl(path)}?v={Guid.NewGuid()}";
    }
}
