using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes;

namespace VokimiShared.src.models.dtos.draft_tests
{
    public class QuestionBriefInfoDto
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; init; }
        public ushort AnswersCount { get; init; }
        public bool IsMultipleChoice { get; init; }
        public QuestionBriefInfoDto(DraftTestQuestion question) {
            Id = question.Id;
            Text = question.Text;
            AnswersCount = (ushort)question.Answers.Count;
            IsMultipleChoice = question.IsMultipleChoice;
        }
    }
}
