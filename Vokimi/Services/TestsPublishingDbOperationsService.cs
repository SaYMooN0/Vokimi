using Amazon.Runtime.Internal.Transform;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.dtos;

namespace Vokimi.Services
{
    public class TestsPublishingDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TestsPublishingDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public async Task<List<TestPublishingProblemDto>> CheckDraftTestForProblems(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            return test switch {
                null => [new("General", "Unable to find the test")],
                DraftGenericTest genericTest => CheckProblemsForGenericTest(genericTest),
                _ => throw new NotImplementedException()
            };

        }
        private List<TestPublishingProblemDto> CheckProblemsForGenericTest(DraftGenericTest test) {
            List<TestPublishingProblemDto> problems =
                CheckTestMainInfoForProblems(test.MainInfo)
                .ToList();

            //problems.AddRange(...);

            //questions

            problems.AddRange(CheckTestResultsForProblems(test.PossibleResults));
            problems.AddRange(CheckQuestionsProblemsForGenericTest(test.Questions.ToList()));

            //styles
            return problems;
        }
        private IEnumerable<TestPublishingProblemDto> CheckTestMainInfoForProblems(DraftTestMainInfo mainInfo) {
            List<string> problems = [];
            if (
                string.IsNullOrWhiteSpace(mainInfo.Name) ||
                mainInfo.Name.Length > TestCreationConsts.MaxTestNameLength ||
                mainInfo.Name.Length < TestCreationConsts.MinTestNameLength) {
                problems.Add($"Name of the test must be from {TestCreationConsts.MinTestNameLength} to {TestCreationConsts.MaxTestNameLength} characters");
            }
            if (!string.IsNullOrEmpty(mainInfo.Description) && mainInfo.Description.Length > TestCreationConsts.MaxTestDescriptionLength) {
                problems.Add($"Description of the test cannot be more than {TestCreationConsts.MaxTestDescriptionLength} characters");
            }
            return problems.Select(TestPublishingProblemDto.NewMainInfoArea);
        }
        private IEnumerable<TestPublishingProblemDto> CheckTestResultsForProblems(ICollection<DraftTestResult> results) {
            List<string> problems = [];
            if (results.Count < 2) { problems.Add("Test cannot have less than two results"); }
            else if (results.Count > TestCreationConsts.MaxResultsForTestCount) {
                problems.Add($"Test cannot have more than {TestCreationConsts.MaxResultsForTestCount} results");
            }
            foreach (var result in results) {
                if (result.AnswersLeadingToResult.Count < 1) {
                    problems.Add($"Result with id: '{result.Id}' has no answers leading to it");
                }
                int textLength = string.IsNullOrWhiteSpace(result.Text) ? 0 : result.Text.Length;

                if (textLength < TestCreationConsts.ResultMinTextLength ||
                    textLength > TestCreationConsts.ResultMaxTextLength) {

                    problems.Add(
$"Text of the result with id: '{result.Id}' is {textLength} characters long. The length must be " +
$"from {TestCreationConsts.ResultMinTextLength} to {TestCreationConsts.ResultMaxTextLength} characters");
                }
            }

            return problems.Select(TestPublishingProblemDto.NewResultsArea);
        }
        private IEnumerable<TestPublishingProblemDto> CheckQuestionsProblemsForGenericTest(List<DraftTestQuestion> questions) {
            List<string> problems = [];
            for (int i = 0; i < questions.Count; i++) {
                DraftTestQuestion q = questions[i];
            }
            foreach (var q in questions) {
                //text
                //is multiple...
                //answers
            }
            return problems.Select(TestPublishingProblemDto.NewQuestionsArea);
        }
        private IEnumerable<TestPublishingProblemDto> CheckQuestionAnswersForProblems(
            ICollection<DraftTestAnswer> answers, int questionNumber) {

            List<string> problems = [];
            foreach (var a in answers) {

            }
            return problems.Select(TestPublishingProblemDto.NewQuestionsArea);
        }
        public async Task<Err> PublishDraftTest(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) {
                return new("Unknown test");
            }
            if (test.Template == TestTemplate.Generic && test is DraftGenericTest genericTest)
                return await PublishGenericDraftTest(genericTest);
            else
                throw new NotImplementedException();
        }
        private async Task<Err> PublishGenericDraftTest(DraftGenericTest test) {
            throw new NotImplementedException();
        }
    }
}
