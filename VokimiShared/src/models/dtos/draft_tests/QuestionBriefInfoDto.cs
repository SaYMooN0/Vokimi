using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.dtos.draft_tests
{
    public class QuestionBriefInfoDto
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; init; }
        public ushort AnswersCount { get; init; }
        public bool IsMultipleChoice { get; init; }
        public QuestionBriefInfoDto(DraftGenericTestQuestion question) {
            Id = question.Id;
            Text = question.Text;
            AnswersCount = (ushort)question.Answers.Count;
            IsMultipleChoice = question.IsMultipleChoice;
        }
    }
}
