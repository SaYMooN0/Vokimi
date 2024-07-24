using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
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

            problems.AddRange(CheckTestResultsForProblems(test.PossibleResults));
            problems.AddRange(CheckQuestionsProblemsForGenericTest(test.Questions.ToList()));

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
                    problems.Add($"Result with id: '{result.StringId}' has no answers leading to it");
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
                Func<string, string> withErrPrefix = (string err) => $"Question #{i + 1}: {err}";

                int textLen = string.IsNullOrWhiteSpace(q.Text) ? 0 : q.Text.Length;

                if (textLen < TestCreationConsts.QuestionTextMinLength || textLen > TestCreationConsts.QuestionTextMaxLength) {
                    problems.Add(withErrPrefix($"Text of the question is {textLen} characters long. The length must be from {TestCreationConsts.QuestionTextMinLength} to {TestCreationConsts.QuestionTextMaxLength} characters"));
                }


                problems.AddRange(CheckQuestionAnswersForProblems(q.Answers, withErrPrefix));

                if (q.IsMultipleChoice) {
                    if (q.MultipleChoiceData is null) {
                        problems.Add(withErrPrefix("question is multiple choice but don't have needed multiple choice data"));
                    }
                    else {
                        if (q.MultipleChoiceData.MaxAnswers - 1 > q.Answers.Count) {
                            problems.Add(withErrPrefix("maximum number of answers must be at least 1 less than the total number of answers"));
                        }
                        if (q.MultipleChoiceData.MinAnswers > q.Answers.Count ||
                            q.MultipleChoiceData.MinAnswers < 1 ||
                            q.MultipleChoiceData.MinAnswers > q.MultipleChoiceData.MaxAnswers) {
                            problems.Add(withErrPrefix("minimum number of answers must be at least 1 and cannot be more than maximum or total number of answers"));
                        }
                    }
                }
            }
            return problems.Select(TestPublishingProblemDto.NewQuestionsArea);
        }
        private IEnumerable<string> CheckQuestionAnswersForProblems(
            ICollection<DraftTestAnswer> answers, Func<string, string> errPrefix) =>
            (answers.Select(a => a.AdditionalInfo) switch {
                IEnumerable<TextOnlyAnswerAdditionalInfo> textOnlyAnswers => CheckTextOnlyAnswersForProblems(textOnlyAnswers),
                IEnumerable<ImageOnlyAnswerAdditionalInfo> imageOnlyAnswers => CheckImageOnlyAnswersForProblems(imageOnlyAnswers),
                IEnumerable<TextAndImageAnswerAdditionalInfo> textAndImageAnswers => CheckTextAndImageAnswersForProblems(textAndImageAnswers),
                _ => throw new InvalidOperationException("Unknown answer type")
            }).Select(s => errPrefix(s));
        private IEnumerable<string> CheckTextOnlyAnswersForProblems(IEnumerable<TextOnlyAnswerAdditionalInfo> infoList) {
            foreach (var a in infoList) {
                int textLen = string.IsNullOrWhiteSpace(a.Text) ? 0 : a.Text.Length;
                if (textLen < TestCreationConsts.AnswerTextMinLength || textLen > TestCreationConsts.AnswerTextMaxLength) {
                    yield return $"Text of the answer is {textLen} characters long. The length must be from {TestCreationConsts.AnswerTextMinLength} to {TestCreationConsts.AnswerTextMaxLength} characters";
                }
            }
        }
        private IEnumerable<string> CheckImageOnlyAnswersForProblems(IEnumerable<ImageOnlyAnswerAdditionalInfo> infoList) {
            foreach (var a in infoList) {
                if (string.IsNullOrWhiteSpace(a.ImagePath)) {
                    yield return "Answer must contain image";
                }
            }
        }
        private IEnumerable<string> CheckTextAndImageAnswersForProblems(IEnumerable<TextAndImageAnswerAdditionalInfo> infoList) {
            foreach (var a in infoList) {
                int textLen = string.IsNullOrWhiteSpace(a.Text) ? 0 : a.Text.Length;
                if (textLen < TestCreationConsts.AnswerTextMinLength || textLen > TestCreationConsts.AnswerTextMaxLength) {
                    yield return $"Text of the answer is {textLen} characters long. The length must be from {TestCreationConsts.AnswerTextMinLength} to {TestCreationConsts.AnswerTextMaxLength} characters";
                }
                if (string.IsNullOrWhiteSpace(a.ImagePath)) {
                    yield return "Answer must contain image";
                }
            }
        }
        public async Task<Err> PublishDraftTest(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) { return new("Unknown test"); }

            if (test.Template == TestTemplate.Generic && test is DraftGenericTest genericTest)
                return await PublishGenericDraftTest(genericTest);
            else
                throw new NotImplementedException();
        }
        private async Task<Err> PublishGenericDraftTest(DraftGenericTest test) {

            //TestGenericType test = TestGenericType.CreateNewFromDraft(); //new conclusion and cover paths
            throw new NotImplementedException();
        }
    }
}
