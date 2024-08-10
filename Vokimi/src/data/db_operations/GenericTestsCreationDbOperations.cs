using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.form_classes.draft_tests_answers_form;
using VokimiShared.src.models.form_classes;
using VokimiShared.src.enums;

namespace Vokimi.src.data.db_operations
{
    internal static class GenericTestsCreationDbOperations
    {
        internal static async Task<Err> CreateNewQuestion(VokimiDbContext db, DraftGenericTest test, string questionText, AnswersType answersType) {
            try {
                ushort questionOrder = (ushort)test.Questions.Count;
                var draftTestQuestion = DraftGenericTestQuestion.CreateNew(questionText, answersType, test.Id, questionOrder);
                test.Questions.Add(draftTestQuestion);
                await db.SaveChangesAsync();
                return Err.None;
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
        }
        internal static async Task<Err> UpdateDraftTestGenericQuestion(VokimiDbContext db, DraftTestQuestionId questionId, QuestionEditingForm newData) {
            DraftGenericTestQuestion? question = await db.DraftGenericTestQuestions.FirstOrDefaultAsync(i => i.Id == questionId);
            if (question is null) { return new Err("Unknown question"); }

            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    ClearAnswersForDraftTestQuestion(db, question);

                    List<DraftGenericTestAnswer> answers = new();

                    foreach (var answerForm in newData.Answers) {
                        if (answerForm.Validate().NotNone())
                            continue;

                        AnswerTypeSpecificInfo typeSpecificInfo = CreateAnswerTypeSpecificInfo(answerForm);

                        ushort orderIndex = (ushort)newData.Answers.IndexOf(answerForm);
                        DraftGenericTestAnswer answer = DraftGenericTestAnswer.CreateNew(questionId, orderIndex, typeSpecificInfo.Id);

                        foreach (DraftTestResultId resultId in answerForm.RelatedResults.Keys) {
                            DraftTestResult? result = await db.DraftTestResults.FirstOrDefaultAsync(i => i.Id == resultId);
                            if (result is not null && result.TestTypeSpecificData is DraftGenericTestResultData resultData) {

                                answer.RelatedResultsData.Add(resultData);
                                resultData.AnswersLeadingToResult.Add(answer);
                            }
                        }

                        db.AnswerTypeSpecificInfo.Add(typeSpecificInfo);
                        db.Add(answer);
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

                    db.DraftGenericTestQuestions.Update(question);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        private static AnswerTypeSpecificInfo CreateAnswerTypeSpecificInfo(BaseAnswerForm answerForm) {
            return answerForm switch {
                ImageOnlyAnswerForm imageOnlyAnswerForm => ImageOnlyAnswerAdditionalInfo.CreateNew(imageOnlyAnswerForm.ImagePath),
                TextAndImageAnswerForm textAndImageAnswerForm => TextAndImageAnswerAdditionalInfo.CreateNew(textAndImageAnswerForm.Text, textAndImageAnswerForm.ImagePath),
                TextOnlyAnswerForm textOnlyAnswerForm => TextOnlyAnswerAdditionalInfo.CreateNew(textOnlyAnswerForm.Text),
                _ => throw new InvalidOperationException("Unknown answer type")
            };
        }
        private static void ClearAnswersForDraftTestQuestion(VokimiDbContext db, DraftGenericTestQuestion question) {
            foreach (var answer in question.Answers) {
                db.AnswerTypeSpecificInfo.Remove(answer.AdditionalInfo);
                db.DraftGenericTestAnswers.Remove(answer);
            }
            db.SaveChanges();
        }
        internal static async Task<Err> DeleteDraftTestQuestion(VokimiDbContext db, DraftTestQuestionId questionId) {
            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    DraftGenericTestQuestion? question = await db.DraftGenericTestQuestions.FirstOrDefaultAsync(i => i.Id == questionId);
                    if (question is null) {
                        return new Err("Unknown question");
                    }
                    ClearAnswersForDraftTestQuestion(db, question);
                    db.DraftGenericTestQuestions.Remove(question);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Err.None;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }
        }
        internal static async Task MoveQuestionUpInOrder(
            VokimiDbContext db, DraftTestId testId, DraftTestQuestionId questionId) {
            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    DraftGenericTest? test = await db.DraftGenericTests.FirstOrDefaultAsync(i => i.Id == testId);
                    if (test is null) {
                        return;
                    }
                    DraftGenericTestQuestion? questionToMoveUp = test.Questions.FirstOrDefault(i => i.Id == questionId);
                    if (questionToMoveUp is null) {
                        return;
                    }
                    if (questionToMoveUp.OrderInTest == 0) {
                        return;
                    }
                    ushort questionToMoveUpCurrentOrder = questionToMoveUp.OrderInTest;
                    DraftGenericTestQuestion? questionToMoveDown =
                        test.Questions.FirstOrDefault(q => q.OrderInTest == questionToMoveUpCurrentOrder - 1);
                    if (questionToMoveDown is not null) {
                        questionToMoveDown.UpdateOrderInTest(questionToMoveUpCurrentOrder);
                        db.DraftGenericTestQuestions.Update(questionToMoveDown);
                    }
                    questionToMoveUp.UpdateOrderInTest((ushort)(questionToMoveUpCurrentOrder - 1));
                    db.DraftGenericTestQuestions.Update(questionToMoveUp);

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
        }
        internal static async Task MoveQuestionDownInOrder(
           VokimiDbContext db, DraftTestId testId, DraftTestQuestionId questionId) {
            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    DraftGenericTest? test = await db.DraftGenericTests.FirstOrDefaultAsync(i => i.Id == testId);
                    if (test is null) {
                        return;
                    }
                    DraftGenericTestQuestion? questionToMoveDown = test.Questions.FirstOrDefault(i => i.Id == questionId);
                    if (questionToMoveDown is null) {
                        return;
                    }
                    if (questionToMoveDown.OrderInTest == test.Questions.Count - 1) {
                        return;
                    }
                    ushort questionToMoveDownCurrentOrder = questionToMoveDown.OrderInTest;
                    DraftGenericTestQuestion? questionToMoveUp =
                        test.Questions.FirstOrDefault(q => q.OrderInTest == questionToMoveDownCurrentOrder + 1);
                    if (questionToMoveUp is not null) {
                        questionToMoveUp.UpdateOrderInTest(questionToMoveDownCurrentOrder);
                        db.DraftGenericTestQuestions.Update(questionToMoveUp);
                    }
                    questionToMoveDown.UpdateOrderInTest((ushort)(questionToMoveDownCurrentOrder + 1));
                    db.DraftGenericTestQuestions.Update(questionToMoveDown);
                    
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                }
            }
        }
    }

}
