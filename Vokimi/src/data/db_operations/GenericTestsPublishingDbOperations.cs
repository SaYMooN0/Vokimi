﻿using Microsoft.EntityFrameworkCore;
using OneOf;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
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
            List<TestPublishingProblemDto> problems =
                CheckTestMainInfoForProblems(test.MainInfo)
                .ToList();

            problems.AddRange(CheckQuestionsForProblems(test.Questions.ToList()));
            problems.AddRange(CheckResultsForProblems(test.PossibleResults));

            return problems;

        }
        private static IEnumerable<TestPublishingProblemDto> CheckTestMainInfoForProblems(DraftTestMainInfo mainInfo) {
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
        private static IEnumerable<TestPublishingProblemDto> CheckResultsForProblems(ICollection<DraftTestResult> results) {
            List<string> problems = [];
            if (results.Count < 2) { problems.Add("Test cannot have less than two results"); }
            else if (results.Count > BaseTestCreationConsts.MaxResultsForTestCount) {
                problems.Add($"Test cannot have more than {BaseTestCreationConsts.MaxResultsForTestCount} results");
            }
            foreach (var result in results) {
                if (result.TestTypeSpecificData is DraftGenericTestResultData resultData) {
                    if (resultData.AnswersLeadingToResult.Count == 0) {
                        problems.Add($"Result with id: '{result.StringId}' has no answers leading to it");
                    }
                }
                else {
                    problems.Add($"Result with id: '{result.StringId}' has been saved incorrectly. Please delete it and try again");
                    continue;
                }
                int textLength = string.IsNullOrWhiteSpace(result.Text) ? 0 : result.Text.Length;

                if (textLength < BaseTestCreationConsts.ResultMinTextLength ||
                    textLength > BaseTestCreationConsts.ResultMaxTextLength) {

                    problems.Add(
$"Text of the result with id: '{result.StringId}' is {textLength} characters long. The length must be " +
$"from {BaseTestCreationConsts.ResultMinTextLength} to {BaseTestCreationConsts.ResultMaxTextLength} characters");
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
        internal static async Task<OneOf<TestGenericType, Err>> PublishGenericDraftTest(VokimiDbContext db, DraftGenericTest test) {
       

            throw new NotImplementedException();
        }
    }
}
