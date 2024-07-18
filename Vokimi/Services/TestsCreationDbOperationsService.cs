using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using System.Collections.Immutable;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.form_classes;
using VokimiShared.src.models.form_classes.draft_tests_answers_form;
using VokimiShared.src.models.form_classes.result_editing;

namespace Vokimi.Services
{
    public class TestsCreationDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TestsCreationDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public async Task<OneOf<DraftTestId, Err>> CreateNewDraftTest(string testName, TestTemplate template, AppUser creator) {
            DraftTestMainInfo mainInfo = DraftTestMainInfo.CreateNewFromName(testName);
            TestStylesSheet styles = TestStylesSheet.CreateNew();
            DraftGenericTest test = DraftGenericTest.CreateNew(creator.Id, mainInfo.Id, styles.Id);

            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    await _db.DraftTestMainInfo.AddAsync(mainInfo);
                    await _db.TestStyles.AddAsync(styles);
                    await _db.SaveChangesAsync();

                    switch (template) {
                        case TestTemplate.Generic:
                            await _db.DraftGenericTests.AddAsync(test);
                            break;
                        case TestTemplate.Knowledge:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentException("Invalid template type");
                    }
                    await _db.SaveChangesAsync();

                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err(ex);
                }
            }

            return test.Id;
        }
        public async Task<TestTemplate?> GetTestTypeById(DraftTestId id) =>
            (await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id))?.Template;
        public async Task<BaseDraftTest> GetDraftTestById(DraftTestId id) =>
            await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
        public async Task<DraftTestMainInfo?> GetDraftTestMainInfoById(DraftTestMainInfoId id) =>
            await _db.DraftTestMainInfo.FirstOrDefaultAsync(mi => mi.Id == id);
        public async Task<List<DraftTestQuestion>> GetDraftTestQuestionsById(DraftTestId id) =>
            await _db.DraftTestQuestions.Where(q => q.DraftTestId == id).ToListAsync();
        public async Task<Err> UpdateTestCover(DraftTestMainInfoId mainInfoId, string newPath) {
            var mainInfo = await GetDraftTestMainInfoById(mainInfoId);
            if (mainInfo is null) {
                return new Err("Test cannot be found");
            }
            if (mainInfo.CoverImagePath == newPath) {
                return Err.None;
            }
            mainInfo.UpdateCoverImage(newPath);
            try {
                _db.DraftTestMainInfo.Update(mainInfo);
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;

        }
        public async Task<Err> UpdateDraftTestMainInfo(
            DraftTestMainInfoId mainInfoId,
            string newName,
            string? newDescription,
            Language newLang,
            TestPrivacy newPrivacy) {
            DraftTestMainInfo? info = await GetDraftTestMainInfoById(mainInfoId);
            if (info is null) {
                return new Err("Unknown test, please refresh the page");
            }
            info.Update(newName, newDescription, newLang, newPrivacy);
            try {
                _db.DraftTestMainInfo.Update(info);
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;
        }
        public async Task<Err> AddQuestionToGenericTest(DraftTestId testId, DraftTestQuestion draftTestQuestion) {
            DraftGenericTest? test = await GetDraftTestById(testId) as DraftGenericTest;
            if (test is null) {
                return new Err("Unknown test");
            }

            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    test.Questions.Add(draftTestQuestion);
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        public Task<DraftTestQuestion?> GetDraftTestQuestionById(DraftTestQuestionId id) =>
            _db.DraftTestQuestions.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<Err> UpdateDraftTestQuestion(DraftTestQuestionId questionId, QuestionEditingForm newData) {
            DraftTestQuestion? question = await GetDraftTestQuestionById(questionId);
            if (question is null) { return new Err("Unknown question"); }

            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    await ClearAnswersForDraftTestQuestion(questionId);
                    List<BaseDraftTestAnswer> answers = new();

                    Dictionary<string, DraftTestResultId> results = _db.DraftTestResults
                       .Where(t => t.TestId == question.DraftTestId)
                       .ToDictionary(res => res.StringId, res => res.Id);

                    foreach (var answerForm in newData.Answers) {
                        if (answerForm.Validate().NotNone())
                            continue;

                        ushort orderIndex = (ushort)newData.Answers.IndexOf(answerForm);
                        BaseDraftTestAnswer answer = answerForm switch {
                            ImageOnlyAnswerForm imageOnlyAnswerForm => DraftTestImageOnlyAnswer
                                .CreateNew(questionId, orderIndex, imageOnlyAnswerForm.ImagePath),
                            TextAndImageAnswerForm textAndImageAnswerForm => DraftTestTextAndImageAnswer
                                .CreateNew(questionId, orderIndex, textAndImageAnswerForm.Text, textAndImageAnswerForm.ImagePath),
                            TextOnlyAnswerForm textOnlyAnswerForm => DraftTestTextOnlyAnswer
                                .CreateNew(questionId, orderIndex, textOnlyAnswerForm.Text),
                            _ => throw new InvalidOperationException("Unknown answer type")
                        };


                        foreach (string resultStringId in answerForm.RelatedResultIds) {
                            if (results.TryGetValue(resultStringId, out DraftTestResultId resultId)) {
                                DraftTestResult? result = await GetDraftTestResultById(resultId);

                                if (result is null) {
                                    continue;
                                }

                                answer.RelatedResults.Add(result);
                            }
                        }

                        _db.Add(answer);
                        answers.Add(answer);
                    }

                    if (newData.IsMultipleChoice) {
                        MultipleChoiceAdditionalData multiChoiceInfo = new() {
                            MaxAnswers = newData.MaxAnswersCount,
                            MinAnswers = newData.MinAnswersCount,
                        };

                        question.UpdateAsMultipleChoice(newData.Text, newData.ImagePath, newData.ShuffleAnswers, answers, multiChoiceInfo);
                    }
                    else {
                        question.UpdateAsSingleChoice(newData.Text, newData.ImagePath, newData.ShuffleAnswers, answers);
                    }

                    _db.DraftTestQuestions.Update(question);
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        public async Task<Err> ClearAnswersForDraftTestQuestion(DraftTestQuestionId questionId) {
            DraftTestQuestion? question = await GetDraftTestQuestionById(questionId);
            if (question is null) { return new Err("Unknown question"); }
            question.Answers.Clear();
            return Err.None;
        }
        public async Task<Err> CreateNewDraftTestResult(DraftTestId testId, string resultId) {
            var result = DraftTestResult.CreateNew(resultId, testId);
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    test.PossibleResults.Add(result);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later.");
                }
            }

            return Err.None;
        }
        public async Task<DraftTestResult?> GetDraftTestResultById(DraftTestResultId id) =>
            await _db.DraftTestResults.FirstOrDefaultAsync(i => i.Id == id);
        public async Task<OneOf<List<string>, Err>> GetResultStringIdsForDraftTest(DraftTestId testId) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("No test found");
            }
            return test.PossibleResults.Select(r => r.StringId).ToList();
        }
        public async Task<Err> UpdateDraftTestConclusion(DraftTestId testId, ConclusionCreationForm data) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            if (test.Conclusion is null) {
                TestConclusion conclusion = TestConclusion.CreateNew(data);
                test.AddConclusion(conclusion);
                _db.DraftTestsSharedInfo.Update(test);
            }
            else {
                test.Conclusion.Update(data);
                _db.TestConclusions.Update(test.Conclusion);
            }

            try {
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later.");
            }

            return Err.None;

        }
        public async Task<Err> RemoveDraftTestConclusion(DraftTestId testId) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            try {
                if (test.ConclusionId is not null && test.Conclusion is not null) {
                    _db.TestConclusions.Remove(test.Conclusion);
                }
                test.RemoveConclusion();

                _db.DraftTestsSharedInfo.Update(test);
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later.");
            }
            return Err.None;
        }
        public async Task<TestConclusion?> GetDraftTestConclusionById(DraftTestId testId) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return null;
            }
            return test.Conclusion;
        }
        public async Task<Err> UpdateDraftTestResults(DraftTestId testId,
            List<ResultWithSaveIdForm> savedResults,
            List<NotSavedResultForm> notSavedResults) {
            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    BaseDraftTest? test = await GetDraftTestById(testId);
                    if (test is null) {
                        return new Err("Unknown test");
                    }

                    var resultsToRemove = new List<DraftTestResult>();

                    ImmutableDictionary<DraftTestResultId, ResultWithSaveIdForm> savedResultsIds =
                        savedResults.ToImmutableDictionary(r => r.Id, r => r);

                    foreach (var r in test.PossibleResults) {
                        if (savedResultsIds.TryGetValue(r.Id, out ResultWithSaveIdForm newData)) {
                            r.Update(newData.Text, newData.ImagePath);
                        }
                        else {
                            resultsToRemove.Add(r);
                        }
                    }
                    foreach (var newRes in notSavedResults) {
                        DraftTestResult r = DraftTestResult.CreateNew(
                            newRes.ResultStringId,
                            testId,
                            newRes.Text,
                            newRes.ImagePath
                            );
                        test.PossibleResults.Add(r);
                    }

                    foreach (var r in resultsToRemove) {
                        await DeleteDraftTestResult(r);
                    }


                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        private async Task<Err> DeleteDraftTestResult(DraftTestResult result) {

            try {
                foreach (var answer in result.AnswersLeadingToResult.ToList()) {
                    answer.RelatedResults.Remove(result);
                }

                _db.DraftTestResults.Remove(result);

                await _db.SaveChangesAsync();
                return Err.None;
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
        }
        public async Task<OneOf<TestStylesSheet, Err>> GetDraftTestStylesByDraftTestId(DraftTestId testId) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            return test.StylesSheet;
        }
        public async Task<Err> UpdateStylesForDraftTest(DraftTestId testId, TestStylesEditingForm formData) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            test.StylesSheet.Update(formData.AccentColor, formData.ArrowsType);
            _db.TestStyles.Update(test.StylesSheet);
            await _db.SaveChangesAsync();
            return Err.None;
        }
    }
}
