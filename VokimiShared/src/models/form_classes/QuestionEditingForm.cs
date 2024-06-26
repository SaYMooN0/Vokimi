﻿using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.questions;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
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
        public bool UseAverageScore { get; set; }
        public List<BaseAnswerForm> Answers { get; set; }
        public static QuestionEditingForm FromDraftTestQuestion(DraftTestQuestion question) => new() {
            Text = question.Text,
            ImagePath = question.ImagePath,
            AnswersType = question.AnswersType,
            ShuffleAnswers = question.ShuffleAnswers,
            IsMultipleChoice = question.IsMultipleChoice,
            MinAnswersCount = ExtractMinAnswersCount(question),
            MaxAnswersCount = ExtractMaxAnswersCount(question),
            UseAverageScore = question.MultipleChoiceData is not null ? question.MultipleChoiceData.UseAverageScore : true,
            Answers = ExtractAnswers(question)
        };

        private static ushort ExtractMinAnswersCount(DraftTestQuestion q) =>
           q.MultipleChoiceData is not null ? (ushort)q.MultipleChoiceData.MinAnswers : (ushort)1;
        private static ushort ExtractMaxAnswersCount(DraftTestQuestion q) =>
           q.MultipleChoiceData is not null ? (ushort)q.MultipleChoiceData.MaxAnswers : (ushort)3;
        private static List<BaseAnswerForm> ExtractAnswers(DraftTestQuestion q) {
            List<BaseAnswerForm> answers = [];

            foreach (var answer in q.Answers) {
                switch (q.AnswersType) {
                    case AnswersType.ImageOnly:
                        var imageOnlyAnswer = answer as ImageOnlyAnswer;
                        if (imageOnlyAnswer != null) {
                            answers.Add(new ImageOnlyAnswerForm {
                                Points = imageOnlyAnswer.Points,
                                ImagePath = imageOnlyAnswer.ImagePath
                            });
                        }
                        break;
                    case AnswersType.TextAndImage:
                        var textAndImageAnswer = answer as TextAndImageAnswer;
                        if (textAndImageAnswer != null) {
                            answers.Add(new TextAndImageAnswerForm {
                                Points = textAndImageAnswer.Points,
                                Text = textAndImageAnswer.Text,
                                ImagePath = textAndImageAnswer.ImagePath
                            });
                        }
                        break;
                    case AnswersType.TextOnly:
                        var textOnlyAnswer = answer as TextOnlyAnswer;
                        if (textOnlyAnswer != null) {
                            answers.Add(new TextOnlyAnswerForm {
                                Points = textOnlyAnswer.Points,
                                Text = textOnlyAnswer.Text
                            });
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Unknown answer type");
                }
            }
            return answers;
        }
    }

}
