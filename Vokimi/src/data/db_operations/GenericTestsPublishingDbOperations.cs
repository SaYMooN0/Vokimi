using Microsoft.EntityFrameworkCore;
using OneOf;
using VokimiShared.src;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_questions;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.dtos;

namespace Vokimi.src.data.db_operations
{
    internal static class GenericTestsPublishingDbOperations
    {
        internal static async Task<List<TestPublishingProblemDto>> CheckProblemsForGenericTest(VokimiDbContext db, DraftTestId id) {
            DraftGenericTest? test = await db.DraftGenericTests.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) {
                return [new("General", "Unable to find the test")];
            }
            List<TestPublishingProblemDto> problems =BaseTestPublishingDbOperations.CheckTestMainInfoForProblems(test.MainInfo)
                .ToList();

            problems.AddRange(CheckQuestionsForProblems(test.Questions.ToList()));
            problems.AddRange(CheckResultsForProblems(test.PossibleResults));

            return problems;

        }
       
        private static IEnumerable<TestPublishingProblemDto> CheckResultsForProblems(ICollection<DraftTestResult> results) {
            List<string> problems = [];
            string? resCountProblem = BaseTestPublishingDbOperations.CheckTestForResultsCount(results);
            if (resCountProblem is not null) {
                problems.Add(resCountProblem);
            }
            foreach (var result in results) {
                if (result.TestTypeSpecificData is DraftGenericTestResultData resultData) {
                    if (resultData.AnswersLeadingToResult.Count == 0) {
                        problems.Add($"Result with id: '{result.StringId}' has no answers leading to it");
                    }
                }
                else {
                    problems.Add($"Result with id: '{result.StringId}' has been saved incorrectly. Please recreate it");
                    continue;
                }

                string? lengthProblem = BaseTestPublishingDbOperations.CheckDraftTestResultForTextLength(result);
                if (lengthProblem is not null) {
                    problems.Add(lengthProblem);
                }
            }
            return problems.Select(TestPublishingProblemDto.NewResultsArea);
        }
        private static IEnumerable<TestPublishingProblemDto> CheckQuestionsForProblems(List<DraftGenericTestQuestion> questions) {
            List<string> problems = [];
            for (int i = 0; i < questions.Count; i++) {
                DraftGenericTestQuestion q = questions[i];
                Func<string, string> withErrPrefix = (string err) => $"Question #{i + 1}: {err}";

                int textLen = string.IsNullOrWhiteSpace(q.Text) ? 0 : q.Text.Length;

                if (textLen < GenericTestCreationConsts.QuestionTextMinLength || textLen > GenericTestCreationConsts.QuestionTextMaxLength) {
                    problems.Add(withErrPrefix($"Text of the question is {textLen} characters long. The length must be from {GenericTestCreationConsts.QuestionTextMinLength} to {GenericTestCreationConsts.QuestionTextMaxLength} characters"));
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
        private static IEnumerable<string> CheckQuestionAnswersForProblems(
       ICollection<DraftGenericTestAnswer> answers, Func<string, string> errPrefix) {
            var problems = new List<string>();

            foreach (var answer in answers) {
                string? answerProblem = answer.AdditionalInfo switch {
                    TextOnlyAnswerAdditionalInfo textOnlyInfo => CheckTextOnlyAnswerForProblems(textOnlyInfo),
                    ImageOnlyAnswerAdditionalInfo imageOnlyInfo => CheckImageOnlyAnswerForProblems(imageOnlyInfo),
                    TextAndImageAnswerAdditionalInfo textAndImageInfo => CheckTextAndImageAnswerForProblems(textAndImageInfo),
                    _ => throw new InvalidOperationException("Unknown answer type")
                };

                if (!string.IsNullOrEmpty(answerProblem)) {
                    problems.Add(errPrefix(answerProblem));
                }
            }

            return problems;
        }

        private static string? CheckTextOnlyAnswerForProblems(TextOnlyAnswerAdditionalInfo info) {
            int textLen = string.IsNullOrWhiteSpace(info.Text) ? 0 : info.Text.Length;
            if (textLen < GenericTestCreationConsts.AnswerTextMinLength || textLen > GenericTestCreationConsts.AnswerTextMaxLength) {
                return $"Text of the answer is {textLen} characters long. The length must be from {GenericTestCreationConsts.AnswerTextMinLength} to {GenericTestCreationConsts.AnswerTextMaxLength} characters";
            }
            return null;
        }

        private static string? CheckImageOnlyAnswerForProblems(ImageOnlyAnswerAdditionalInfo info) =>
            string.IsNullOrWhiteSpace(info.ImagePath) ? "Answer must contain image" : null;


        private static string? CheckTextAndImageAnswerForProblems(TextAndImageAnswerAdditionalInfo info) {
            int textLen = string.IsNullOrWhiteSpace(info.Text) ? 0 : info.Text.Length;
            if (textLen < GenericTestCreationConsts.AnswerTextMinLength || textLen > GenericTestCreationConsts.AnswerTextMaxLength) {
                return $"Text of the answer is {textLen} characters long. The length must be from {GenericTestCreationConsts.AnswerTextMinLength} to {GenericTestCreationConsts.AnswerTextMaxLength} characters";
            }
            if (string.IsNullOrWhiteSpace(info.ImagePath)) {
                return "Answer must contain image";
            }
            return null;
        }

        internal static async Task<OneOf<TestGenericType, Err>> PublishGenericDraftTest(VokimiDbContext db, DraftGenericTest draftTest) {
            if ((await CheckProblemsForGenericTest(db, draftTest.Id)).Any()) {
                return new Err("Some problems with the test. Please try again later");
            }

            using var transaction = await db.Database.BeginTransactionAsync();
            try {
                var publishingDto = TestPublishingDto.FromBaseDraftTest(draftTest);

                var publishedResults = await PublishTestResults(db, draftTest, publishingDto.Id);

                var publishedQuestions = await PublishTestQuestions(db, draftTest, publishingDto.Id, publishedResults);

                var testToPublish = TestGenericType.CreateNew(publishingDto, publishedQuestions, publishedResults.Values);
                db.TestsGenericType.Add(testToPublish);

                //await RemoveDraftTestEntries(db, draftTest);

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return testToPublish;
            } catch (Exception ex) {
                await transaction.RollbackAsync();
                return new Err("Error publishing the test. Please try again later.");
            }
        }


        private static async Task<Dictionary<string, GenericTestResult>> PublishTestResults(VokimiDbContext db, DraftGenericTest draftTest, TestId testId) {
            var publishedResults = new Dictionary<string, GenericTestResult>(draftTest.PossibleResults.Count);

            foreach (var draftResult in draftTest.PossibleResults) {
                var resultToPublish = GenericTestResult.CreateNew(testId, draftResult.Text, draftResult.ImagePath);
                db.GenericTestResults.Add(resultToPublish);
                publishedResults.Add(draftResult.StringId, resultToPublish);
            }

            return publishedResults;
        }

        private static async Task<List<GenericTestQuestion>> PublishTestQuestions(VokimiDbContext db, DraftGenericTest draftTest, TestId testId, Dictionary<string, GenericTestResult> publishedResults) {
            var publishedQuestions = new List<GenericTestQuestion>();

            foreach (var draftQuestion in draftTest.Questions) {
                MultiChoiceQuestionDataId? multiChoiceDataId = null;

                if (draftQuestion.IsMultipleChoice) {
                    var multiChoiceData = MultiChoiceQuestionData.CreateNew(draftQuestion.MultipleChoiceData.MinAnswers, draftQuestion.MultipleChoiceData.MaxAnswers);
                    db.MultiChoiceQuestionsData.Add(multiChoiceData);
                    multiChoiceDataId = multiChoiceData.Id;
                }

                var questionToPublish = GenericTestQuestion.CreateNew(testId, draftQuestion.Text, draftQuestion.ImagePath, draftQuestion.AnswersType, multiChoiceDataId);
                db.GenericTestQuestions.Add(questionToPublish);

                foreach (var draftAnswer in draftQuestion.Answers) {
                    ushort order = draftQuestion.ShuffleAnswers ? (ushort)0 : draftAnswer.OrderInQuestion;
                    var relatedResultIds = draftAnswer.RelatedResultsData.Select(r => r.DraftTestResult.StringId);
                    var relatedResults = publishedResults.Where(p => relatedResultIds.Contains(p.Key)).Select(p => p.Value).ToList();
                    var answerToPublish = GenericTestAnswer.CreateNew(questionToPublish.Id, order, draftAnswer.AdditionalInfoId, relatedResults);

                    db.GenericTestAnswers.Add(answerToPublish);
                    db.DraftGenericTestAnswers.Remove(draftAnswer);
                }
                db.DraftGenericTestQuestions.Remove(draftQuestion);

                publishedQuestions.Add(questionToPublish);
            }

            return publishedQuestions;
        }

        internal static async Task RemoveDraftTestEntries(VokimiDbContext db, DraftGenericTest draftTest) {
            foreach (var draftQuestion in draftTest.Questions) {
                foreach (var draftAnswer in draftQuestion.Answers) {
                    db.DraftGenericTestAnswers.Remove(draftAnswer);
                }
                db.DraftGenericTestQuestions.Remove(draftQuestion);
            }

            foreach (var draftResult in draftTest.PossibleResults) {
                db.DraftTestTypeSpecificResultsData.Remove(draftResult.TestTypeSpecificData);
                db.DraftTestResults.Remove(draftResult);
            }
        }

    }
}
