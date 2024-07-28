using Microsoft.EntityFrameworkCore;
using OneOf;
using System.Collections.Immutable;
using VokimiShared.src;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.form_classes;
using VokimiShared.src.models.form_classes.draft_tests_answers_form;
using VokimiShared.src.models.form_classes.result_editing;

namespace Vokimi.src.data.db_operations
{
    internal static class BaseTestsCreationDbOperations
    {
        internal static async Task<OneOf<DraftTestId, Err>> CreateNewDraftTest(VokimiDbContext db, string testName, TestTemplate template, AppUser creator) {
            DraftTestMainInfo mainInfo = DraftTestMainInfo.CreateNewFromName(testName);
            TestStylesSheet styles = TestStylesSheet.CreateNew();
            DraftGenericTest test = DraftGenericTest.CreateNew(creator.Id, mainInfo.Id, styles.Id);

            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    await db.DraftTestMainInfo.AddAsync(mainInfo);
                    await db.TestStyles.AddAsync(styles);
                    await db.SaveChangesAsync();

                    switch (template) {
                        case TestTemplate.Generic:
                            await db.DraftGenericTests.AddAsync(test);
                            break;
                        case TestTemplate.Knowledge:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentException("Invalid template type");
                    }
                    await db.SaveChangesAsync();

                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err(ex);
                }
            }

            return test.Id;
        }

        static async Task<List<DraftGenericTestQuestion>> GetDraftTestQuestionsById(VokimiDbContext db, DraftTestId id) =>
            await db.DraftGenericTestQuestions.Where(q => q.DraftTestId == id).ToListAsync();

        static async Task<Err> UpdateTestCover(VokimiDbContext db, DraftTestId testId, string newPath) {
            BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
            if (test is null) {
                return new Err("Test cannot be found");
            }
            if (test.MainInfo.CoverImagePath == newPath) {
                return Err.None;
            }
            test.MainInfo.UpdateCoverImage(newPath);
            try {
                db.DraftTestMainInfo.Update(test.MainInfo);
                await db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;
        }

        internal static async Task<Err> UpdateDraftTestMainInfo(VokimiDbContext db, DraftTestMainInfoId mainInfoId, string newName, string? newDescription, Language newLang, TestPrivacy newPrivacy) {
            DraftTestMainInfo? info = await  db.DraftTestMainInfo.FirstOrDefaultAsync(mi => mi.Id == mainInfoId);
            if (info is null) {
                return new Err("Unknown test, please refresh the page");
            }
            info.Update(newName, newDescription, newLang, newPrivacy);
            try {
                db.DraftTestMainInfo.Update(info);
                await db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;
        }

        internal static async Task<Err> CreateNewDraftTestResult(VokimiDbContext db, DraftTestId testId, string resultId) {
            BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
            if (test is null) {
                return new Err("Unknown test");
            }

            var specificDataId = await CreateEmptyDraftTestResultData(db, test.Template);
            var result = DraftTestResult.CreateNew(resultId, testId, specificDataId);

            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    test.PossibleResults.Add(result);

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later.");
                }
            }

            return Err.None;
        }
        static async Task<DraftTestTypeSpecificResultDataId> CreateEmptyDraftTestResultData(VokimiDbContext db, TestTemplate testTemplate) {
            switch (testTemplate) {
                case TestTemplate.Generic:
                    var resultData = DraftGenericTestResultData.CreateNew();
                    db.DraftGenericTestResultsData.Add(resultData);
                    await db.SaveChangesAsync();
                    return resultData.Id;
                default:
                    throw new ArgumentException("Invalid template type");
            }
        }
        internal static async Task<Err> UpdateDraftTestConclusion(VokimiDbContext db, DraftTestId testId, ConclusionCreationForm data) {
            BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            if (test.Conclusion is null) {
                TestConclusion conclusion = TestConclusion.CreateNew(data);
                test.AddConclusion(conclusion);
                db.DraftTestsSharedInfo.Update(test);
            }
            else {
                test.Conclusion.Update(data);
                db.TestConclusions.Update(test.Conclusion);
            }

            try {
                await db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later.");
            }

            return Err.None;
        }

        internal static async Task<Err> RemoveDraftTestConclusion(VokimiDbContext db, DraftTestId testId) {
            BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            try {
                if (test.ConclusionId is not null && test.Conclusion is not null) {
                    db.TestConclusions.Remove(test.Conclusion);
                }
                test.RemoveConclusion();

                db.DraftTestsSharedInfo.Update(test);
                await db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later.");
            }
            return Err.None;
        }

        internal static async Task<Err> UpdateDraftTestResults(VokimiDbContext db, DraftTestId testId, List<ResultWithSaveIdForm> savedResults, List<NotSavedResultForm> notSavedResults) {
            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
                    if (test is null) {
                        return new Err("Unknown test");
                    }

                    List<DraftTestResult> resultsToRemove = new();

                    ImmutableDictionary<DraftTestResultId, ResultWithSaveIdForm> savedResultsIds = savedResults.ToImmutableDictionary(r => r.Id, r => r);

                    foreach (var r in test.PossibleResults) {
                        if (savedResultsIds.TryGetValue(r.Id, out ResultWithSaveIdForm newData)) {
                            r.Update(newData.Text, newData.ImagePath);
                        }
                        else {
                            resultsToRemove.Add(r);
                        }
                    }
                    foreach (var newRes in notSavedResults) {
                        var resultTypeSpecificDataId = await CreateEmptyDraftTestResultData(db, test.Template);

                        DraftTestResult r = DraftTestResult.CreateNew(newRes.ResultStringId, testId, newRes.Text, newRes.ImagePath, resultTypeSpecificDataId);
                        test.PossibleResults.Add(r);
                    }

                    foreach (var r in resultsToRemove) {
                        await DeleteDraftTestResult(db, r);
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }

        private static async Task<Err> DeleteDraftTestResult(VokimiDbContext db, DraftTestResult result) {
            try {
                DeleteDraftTestResultAdditionalData(db, result);
                db.DraftTestResults.Remove(result);
                await db.SaveChangesAsync();
                return Err.None;
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
        }

        private static void DeleteDraftTestResultAdditionalData(VokimiDbContext db, DraftTestResult result) {
            switch (result.TestTypeSpecificData) {
                case DraftGenericTestResultData genericTestData:
                    foreach (var answer in genericTestData.AnswersLeadingToResult.ToList()) {
                        answer.RelatedResults.Remove(result);
                    }
                    break;
                default:
                    throw new ArgumentException("Unexpected test type");
            }
        }

        internal static async Task<Err> UpdateStylesForDraftTest(VokimiDbContext db, DraftTestId testId, TestStylesEditingForm formData) {
            BaseDraftTest? test = await db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == testId);
            if (test is null) {
                return new Err("Unknown test");
            }
            test.StylesSheet.Update(formData.AccentColor, formData.ArrowsType);
            db.TestStyles.Update(test.StylesSheet);
            await db.SaveChangesAsync();
            return Err.None;
        }
    }
}
