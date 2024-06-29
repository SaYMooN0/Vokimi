using Microsoft.EntityFrameworkCore;
using OneOf;
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

            } catch (Exception ex) {
                return new Err(ex);
            }
            return test.Id;
        }
        public async Task<TestTemplate?> GetTestTypeById(DraftTestId id) =>
            (await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id))?.Template;
        public async Task<T?> GetTestById<T>(DraftTestId id, TestTemplate template) where T : BaseDraftTest =>
            await _db.Set<T>().FirstOrDefaultAsync(i => i.Id == id && i.Template == template);
        public async Task<DraftTestMainInfo?> GetDraftTestMainInfoById(DraftTestMainInfoId id) =>
            await _db.DraftTestMainInfo.FirstOrDefaultAsync(mi => mi.Id == id);
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
            DraftGenericTest? test = await GetTestById<DraftGenericTest>(testId, TestTemplate.Generic);
            if (test is null) {
                return new Err("Unknown test");
            }
            test.Questions.Add(draftTestQuestion);
            try {
                _db.DraftGenericTests.Update(test);
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;
        }
        public Task<DraftTestQuestion?> GetDraftTestQuestionById(DraftTestQuestionId id) =>
            _db.DraftTestQuestions.FirstOrDefaultAsync(i => i.Id == id);

        public async Task<Err> UpdateDraftTestQuestion(DraftTestQuestionId questionId, QuestionEditingForm newData) {
            DraftTestQuestion? question = await GetDraftTestQuestionById(questionId);
            if (question is null) { return new Err("Unknown question"); }

            await DeleteAnswerForDraftTestQuestion(questionId);
            var answers = new List<BaseAnswer>();

            foreach (var answerForm in newData.Answers) {
                ushort orderIndex = (ushort)newData.Answers.IndexOf(answerForm);
                BaseAnswer answer = answerForm switch {
                    ImageOnlyAnswerForm imageOnlyAnswerForm => ImageOnlyAnswer
                        .CreateNew(questionId, imageOnlyAnswerForm.Points, orderIndex, imageOnlyAnswerForm.ImagePath),
                    TextAndImageAnswerForm textAndImageAnswerForm => TextAndImageAnswer
                        .CreateNew(questionId, textAndImageAnswerForm.Points, orderIndex, textAndImageAnswerForm.Text, textAndImageAnswerForm.ImagePath),
                    TextOnlyAnswerForm textOnlyAnswerForm => TextOnlyAnswer
                        .CreateNew(questionId, textOnlyAnswerForm.Points, orderIndex, textOnlyAnswerForm.Text),
                    _ => throw new InvalidOperationException("Unknown answer type")
                };

                _db.Add(answer);
                answers.Add(answer);
            }

            if (newData.IsMultipleChoice) {
                MultipleChoiceAdditionalData multiChoiceInfo = new() {
                    MaxAnswers = newData.MaxAnswersCount,
                    MinAnswers = newData.MinAnswersCount,
                    UseAverageScore = newData.UseAverageScore,
                };

                question.UpdateAsMultipleChoice(newData.Text, newData.ImagePath, newData.ShuffleAnswers, answers, multiChoiceInfo);
            }
            else {
                question.UpdateAsSingleChoice(newData.Text, newData.ImagePath, newData.ShuffleAnswers, answers);
            }
            try {
                _db.DraftTestQuestions.Update(question);
                await _db.SaveChangesAsync();
                return Err.None;

            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
        }

        public async Task<Err> DeleteAnswerForDraftTestQuestion(DraftTestQuestionId questionId) {
            DraftTestQuestion? question = await GetDraftTestQuestionById(questionId);
            if (question is null) { return new Err("Unknown question"); }
            question.Answers.Clear();
            return Err.None;
        }
    }
}
