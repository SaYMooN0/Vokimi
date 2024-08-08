using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test.test_questions;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test
{
    public class GenericTestQuestion
    {
        public GenericTestQuestionId Id { get; init; }
        public TestId TestId { get; init; }
        public string Text { get; init; }
        public string? ImagePath { get; init; }
        public AnswersType AnswersType { get; init; }
        public ushort OrderInTest { get; init; }

        //if MultiChoiceQuestionData is null question is not multi choice
        public MultiChoiceQuestionDataId? MultiChoiceQuestionDataId { get; init; }
        public virtual MultiChoiceQuestionData? MultiChoiceQuestionData { get; protected set; }


        public virtual ICollection<GenericTestAnswer> Answers { get; protected set; } = [];

        public static GenericTestQuestion CreateNew(TestId testId,
                                                    string text,
                                                    string? imagePath,
                                                    AnswersType answersType,
                                                    ushort orderInTest,
                                                    MultiChoiceQuestionDataId? multiChoiceQuestionDataId) =>
            new() {
                Id = new(),
                TestId = testId,
                Text = text,
                ImagePath = imagePath,
                AnswersType = answersType,
                OrderInTest = orderInTest,
                MultiChoiceQuestionDataId = multiChoiceQuestionDataId
            };

    }
}
