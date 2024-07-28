using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.dtos.draft_tests
{
    public record class QuestionBriefInfoDto(
        DraftTestQuestionId Id,
        string Text,
        ushort AnswersCount,
        bool IsMultipleChoice)
    {
        public static QuestionBriefInfoDto FromDraftGenericTestQuestion(DraftGenericTestQuestion question) =>
            new(
                question.Id,
                question.Text,
                (ushort)question.Answers.Count,
                question.IsMultipleChoice
            );
    }
}
