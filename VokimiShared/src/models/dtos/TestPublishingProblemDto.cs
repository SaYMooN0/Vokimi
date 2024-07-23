
namespace VokimiShared.src.models.dtos
{
    public record class TestPublishingProblemDto(
        string Area,
        string Problem)
    {
        public static TestPublishingProblemDto NewMainInfoArea(string message) =>
            new("Test main info", message);
        public static TestPublishingProblemDto NewQuestionsArea(string message) =>
            new("Test questions", message); 
        public static TestPublishingProblemDto NewResultsArea(string message) =>
            new("Test results", message);
    }
}
