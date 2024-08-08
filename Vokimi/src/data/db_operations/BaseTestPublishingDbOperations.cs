using OneOf.Types;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.dtos;

namespace Vokimi.src.data.db_operations
{
    internal static class BaseTestPublishingDbOperations
    {
        internal static IEnumerable<TestPublishingProblemDto> CheckTestMainInfoForProblems(DraftTestMainInfo mainInfo) {
            List<string> problems = [];
            if (
                string.IsNullOrWhiteSpace(mainInfo.Name) ||
                mainInfo.Name.Length > BaseTestCreationConsts.MaxTestNameLength ||
                mainInfo.Name.Length < BaseTestCreationConsts.MinTestNameLength) {
                problems.Add($"Name of the test must be from {BaseTestCreationConsts.MinTestNameLength} to {BaseTestCreationConsts.MaxTestNameLength} characters");
            }
            if (!string.IsNullOrEmpty(mainInfo.Description) && mainInfo.Description.Length > BaseTestCreationConsts.MaxTestDescriptionLength) {
                problems.Add($"Description of the test cannot be more than {BaseTestCreationConsts.MaxTestDescriptionLength} characters");
            }
            return problems.Select(TestPublishingProblemDto.NewMainInfoArea);
        }
        internal static string? CheckTestForResultsCount(ICollection<DraftTestResult> results) {
            if (results.Count < 2) {
                return "Test cannot have less than two results";
            }
            else if (results.Count > BaseTestCreationConsts.MaxResultsForTestCount) {
                return $"Test cannot have more than {BaseTestCreationConsts.MaxResultsForTestCount} results";
            }
            else { return null; }
        }
        internal static string? CheckDraftTestResultForTextLength(DraftTestResult result) {
            int textLength = string.IsNullOrWhiteSpace(result.Text) ? 0 : result.Text.Length;

            if (textLength < BaseTestCreationConsts.ResultMinTextLength ||
                textLength > BaseTestCreationConsts.ResultMaxTextLength) {
                return $"Text of the result with id: '{result.Name}' is {textLength} characters long. The length must be " +
                       $"from {BaseTestCreationConsts.ResultMinTextLength} to {BaseTestCreationConsts.ResultMaxTextLength} characters";
            }
            else { return null; }
        }
    }
}
