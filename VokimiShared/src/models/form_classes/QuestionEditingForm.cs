using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.form_classes
{
    public class QuestionEditingForm
    {
        public string Text { get;  set; }
        public string? ImagePath { get;  set; }
        public AnswersType AnswersType { get; init; }
        public bool ShuffleAnswers { get;  set; }

        public bool IsMultipleChoice { get;  set; }
        public ushort MinAnswersCount { get;  set; }
        public ushort MaxAnswersCount { get;  set; }
        public bool UseAverageScore { get;  set; }
        //Answers
        public static QuestionEditingForm FromDraftTestQuestion(DraftTestQuestion question) => new() {
            Text = question.Text,
            ImagePath = question.ImagePath,
            AnswersType = question.AnswersType,
            ShuffleAnswers = question.ShuffleAnswers,
            IsMultipleChoice = question.IsMultipleChoice,
            MinAnswersCount = ExtractMinAnswersCount(question),
            MaxAnswersCount = ExtractMaxAnswersCount(question),
            UseAverageScore = question.MultipleChoiceData is not null ? question.MultipleChoiceData.UseAverageScore : true
        };
         static ushort ExtractMinAnswersCount(DraftTestQuestion q) =>
            q.MultipleChoiceData is not null ? (ushort)q.MultipleChoiceData.MinAnswers : (ushort)1;
         static ushort ExtractMaxAnswersCount(DraftTestQuestion q) =>
            q.MultipleChoiceData is not null ? (ushort)q.MultipleChoiceData.MaxAnswers : (ushort)3;
    }

}
