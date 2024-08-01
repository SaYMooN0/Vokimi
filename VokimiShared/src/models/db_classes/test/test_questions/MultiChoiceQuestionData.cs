using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test.test_questions
{
    public class MultiChoiceQuestionData
    {
        public MultiChoiceQuestionDataId Id { get; init; }
        public int MinAnswersCount { get; init; }
        public int MaxAnswersCount { get; init; }
        public static MultiChoiceQuestionData CreateNew(int minAnswersCount, int maxAnswersCount) =>
            new() {
                Id = new(),
                MinAnswersCount = minAnswersCount,
                MaxAnswersCount = maxAnswersCount
            };
    }
}
