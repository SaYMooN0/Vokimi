using Microsoft.EntityFrameworkCore;
using OneOf;
using System.Collections;
using System.Collections.Generic;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.form_classes;
using VokimiShared.src.models.form_classes.draft_tests_answers_form;

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
            DraftGenericTest test = DraftGenericTest.CreateNew(creator.Id, mainInfo.Id);

            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    await _db.DraftTestMainInfo.AddAsync(mainInfo);
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
                    List<BaseAnswer> answers = new();

                    foreach (var answerForm in newData.Answers) {
                        if (answerForm.Validate().NotNone())
                            continue;

                        ushort orderIndex = (ushort)newData.Answers.IndexOf(answerForm);
                        BaseAnswer answer = answerForm switch {
                            ImageOnlyAnswerForm imageOnlyAnswerForm => ImageOnlyAnswer
                                .CreateNew(questionId, orderIndex, imageOnlyAnswerForm.ImagePath),
                            TextAndImageAnswerForm textAndImageAnswerForm => TextAndImageAnswer
                                .CreateNew(questionId, orderIndex, textAndImageAnswerForm.Text, textAndImageAnswerForm.ImagePath),
                            TextOnlyAnswerForm textOnlyAnswerForm => TextOnlyAnswer
                                .CreateNew(questionId, orderIndex, textOnlyAnswerForm.Text),
                            _ => throw new InvalidOperationException("Unknown answer type")
                        };
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
            try {
                BaseDraftTest? test = await GetDraftTestById(testId);
                if (test is null) {
                    return new Err("Unknown test");
                }
                using (var transaction = await _db.Database.BeginTransactionAsync()) {
                    try {
                        _db.DraftTestResults.Add(result);
                        test.PossibleResults.Add(result);

                        await _db.SaveChangesAsync();
                        await transaction.CommitAsync();
                    } catch (Exception ex) {
                        await transaction.RollbackAsync();
                        return new Err("Server error. Please try again later.");
                    }
                }

                return Err.None;
            } catch (Exception ex) {
                return new Err($"Server error. Please try again later.");
            }
        }
        public async Task<DraftTestResult?> GetDraftTestResultById(DraftTestResultId id) =>
            await _db.DraftTestResults.FirstOrDefaultAsync(i => i.Id == id);
        public async Task<Err> AssignDraftTestResultToAnswer<AnswerType>(DraftTestResultId resultId, AnswerId answerId) where AnswerType : BaseAnswer {
            using (var transaction = await _db.Database.BeginTransactionAsync()) {
                try {
                    var result = await GetDraftTestResultById(resultId);
                    if (result is null) {
                        return new Err("Unknown result");
                    }

                    var answer = await _db.AnswersSharedInfo.FirstOrDefaultAsync(a => a.AnswerId == answerId);
                    if (answer is null) {
                        return new Err("Unknown answer");
                    }

                    result.AnswersLeadingToResult.Add(answer);
                    answer.RelatedResults.Add(result);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        public async Task<OneOf<List<string>, Err>> GetResultStringIdsForDraftTest(DraftTestId testId) {
            BaseDraftTest? test = await GetDraftTestById(testId);
            if (test is null) {
                return new Err("No test found");
            }
            return test.PossibleResults.Select(r => r.StringId).ToList();
        }
    }
}
