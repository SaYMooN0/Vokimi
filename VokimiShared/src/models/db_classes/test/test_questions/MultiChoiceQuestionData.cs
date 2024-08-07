using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test.test_questions
{
    public class MultiChoiceQuestionData
    {
        public MultiChoiceQuestionDataId Id { get; init; }
        public ushort MinAnswersCount { get; init; }
        public ushort MaxAnswersCount { get; init; }
        public static MultiChoiceQuestionData CreateNew(ushort minAnswersCount, ushort maxAnswersCount) =>
            new() {
                Id = new(),
                MinAnswersCount = minAnswersCount,
                MaxAnswersCount = maxAnswersCount
            };
    }
}
