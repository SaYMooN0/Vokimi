﻿using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.form_classes.draft_tests_answers_form;

namespace VokimiShared.src.models.form_classes
{
    public class QuestionEditingForm
    {
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public AnswersType AnswersType { get; init; }
        public bool ShuffleAnswers { get; set; }

        public bool IsMultipleChoice { get; set; }
        public ushort MinAnswersCount { get; set; }
        public ushort MaxAnswersCount { get; set; }
        public List<BaseAnswerForm> Answers { get; set; } = [];
        public static QuestionEditingForm FromDraftTestQuestion(DraftGenericTestQuestion question) => new() {
            Text = question.Text,
            ImagePath = question.ImagePath,
            AnswersType = question.AnswersType,
            ShuffleAnswers = question.ShuffleAnswers,
            IsMultipleChoice = question.IsMultipleChoice,
            MinAnswersCount = ExtractMinAnswersCount(question),
            MaxAnswersCount = ExtractMaxAnswersCount(question),
            Answers = ExtractAnswers(question)
        };
        public Err Validate() {
            if (IsMultipleChoice) {
                if (MinAnswersCount > MaxAnswersCount) {
                    return new Err("Minimum answers count cannot be more than maximum answers count");
                }
                else if (MinAnswersCount < 1) {
                    return new Err("Minimum answers count cannot be less than 1");
                }
                else if (MaxAnswersCount > Answers.Count) {
                    return new Err("Maximum answers count cannot be more than total answers count");
                }
            }
            for (int i = 0; i < Answers.Count; i++) {
                Err err = Answers[i].Validate();
                if (err.NotNone()) {
                    return new Err($"Error in answer #{i + 1}: " + err.ToString());
                }
            }
            return Err.None;
        }


        private static ushort ExtractMinAnswersCount(DraftGenericTestQuestion q) =>
           q.MultipleChoiceData is not null ? q.MultipleChoiceData.MinAnswers : (ushort)1;
        private static ushort ExtractMaxAnswersCount(DraftGenericTestQuestion q) =>
           q.MultipleChoiceData is not null ? q.MultipleChoiceData.MaxAnswers : (ushort)3;
        private static List<BaseAnswerForm> ExtractAnswers(DraftGenericTestQuestion q) {
            List<BaseAnswerForm> answers = [];

            foreach (var answer in q.Answers.OrderBy(a => a.OrderInQuestion)) {
                var results = answer.RelatedResultsData.ToDictionary(r => r.DraftTestResult.Id, r => r.DraftTestResult.Name);

                BaseAnswerForm form = q.AnswersType switch {
                    AnswersType.ImageOnly => new ImageOnlyAnswerForm {
                        ImagePath = (answer.AdditionalInfo as ImageOnlyAnswerAdditionalInfo)?.ImagePath,
                        RelatedResults = results
                    },
                    AnswersType.TextAndImage => new TextAndImageAnswerForm {
                        Text = (answer.AdditionalInfo as TextAndImageAnswerAdditionalInfo)?.Text,
                        ImagePath = (answer.AdditionalInfo as TextAndImageAnswerAdditionalInfo)?.ImagePath,
                        RelatedResults = results
                    },
                    AnswersType.TextOnly => new TextOnlyAnswerForm {
                        Text = (answer.AdditionalInfo as TextOnlyAnswerAdditionalInfo)?.Text,
                        RelatedResults = results
                    },
                    _ => throw new InvalidOperationException("Unknown answer type")
                };

                answers.Add(form);
            }

            return answers;
        }


    }

}
