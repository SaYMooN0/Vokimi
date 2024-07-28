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
                var draftTestQuestion= DraftGenericTestQuestion.CreateNew(questionText, answersType, test.Id);
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

                    Dictionary<string, DraftTestResultId> results = db.DraftTestResults
                        .Where(t => t.TestId == question.DraftTestId)
                        .ToDictionary(res => res.StringId, res => res.Id);

                    foreach (var answerForm in newData.Answers) {
                        if (answerForm.Validate().NotNone())
                            continue;

                        AnswerTypeSpecificInfo typeSpecificInfo = answerForm switch {
                            ImageOnlyAnswerForm imageOnlyAnswerForm => ImageOnlyAnswerAdditionalInfo.CreateNew(imageOnlyAnswerForm.ImagePath),
                            TextAndImageAnswerForm textAndImageAnswerForm => TextAndImageAnswerAdditionalInfo.CreateNew(textAndImageAnswerForm.Text, textAndImageAnswerForm.ImagePath),
                            TextOnlyAnswerForm textOnlyAnswerForm => TextOnlyAnswerAdditionalInfo.CreateNew(textOnlyAnswerForm.Text),
                            _ => throw new InvalidOperationException("Unknown answer type")
                        };

                        ushort orderIndex = (ushort)newData.Answers.IndexOf(answerForm);
                        DraftGenericTestAnswer answer = DraftGenericTestAnswer.CreateNew(questionId, orderIndex, typeSpecificInfo.Id);

                        foreach (string resultStringId in answerForm.RelatedResultIds) {
                            if (results.TryGetValue(resultStringId, out DraftTestResultId resultId)) {
                                DraftTestResult? result = await db.DraftTestResults.FirstOrDefaultAsync(i => i.Id == resultId);

                                if (result is null) {
                                    continue;
                                }

                                answer.RelatedResults.Add(result);
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
        private static void ClearAnswersForDraftTestQuestion(VokimiDbContext db, DraftGenericTestQuestion question) {
            foreach (var answer in question.Answers) {
                foreach (var result in answer.RelatedResults) {
                    (result.TestTypeSpecificData as DraftGenericTestResultData).AnswersLeadingToResult.Remove(answer);
                }
                db.AnswerTypeSpecificInfo.Remove(answer.AdditionalInfo);
                db.DraftGenericTestAnswers.Remove(answer);
            }
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
    }

}
