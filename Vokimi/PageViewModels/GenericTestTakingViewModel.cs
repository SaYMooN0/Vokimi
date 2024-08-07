using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.PageViewModels
{
    internal class GenericTestTakingViewModel
    {
        internal ushort CurrentQuestionNumber { get; set; }
        internal ushort TotalNumberOfQuestions { get; init; }
        internal GenericTestTakingQuestionDto[] Questions { get; init; }
        internal string AccentColor { get; init; }
        internal ArrowIconType ArrowIcons { get; init; }
        internal GenericTestTakingQuestionDto CurrentQuestion =>
            CurrentQuestionNumber >= Questions.Length ?
            new(string.Empty, AnswersType.TextOnly, 0, 0, []) :
            Questions[CurrentQuestionNumber];
        internal static GenericTestTakingViewModel FromTest(TestGenericType test) => new() {
            CurrentQuestionNumber = 0,
            TotalNumberOfQuestions = (ushort)test.Questions.Count,
            Questions = test.Questions.Select(GenericTestTakingQuestionDto.FromGenericTestQuestion).ToArray(),
            AccentColor = test.StylesSheet.AccentColor,
            ArrowIcons = test.StylesSheet.ArrowsType
        };
        internal static GenericTestTakingViewModel Empty => new() {
            CurrentQuestionNumber = 0,
            TotalNumberOfQuestions = 0,
            Questions = Array.Empty<GenericTestTakingQuestionDto>()
        };
    }

    internal record class GenericTestTakingQuestionDto(
        string Text,
        AnswersType AnswersType,
        ushort MinAnswersCount,
        ushort MaxAnswersCount,
        GenericTestTakingAnswerDto[] Answers)
    {
        internal static GenericTestTakingQuestionDto FromGenericTestQuestion(GenericTestQuestion question) => new(
            question.Text,
            question.AnswersType,
            question.MultiChoiceQuestionData is null ? (ushort)1 : question.MultiChoiceQuestionData.MinAnswersCount,
            question.MultiChoiceQuestionData is null ? (ushort)1 : question.MultiChoiceQuestionData.MaxAnswersCount,
            GetAnswersDtos(question.AnswersType, question.Answers));
        private static GenericTestTakingAnswerDto[] GetAnswersDtos(AnswersType type, IEnumerable<GenericTestAnswer> answers) =>
            type switch {
                AnswersType.TextOnly => answers
                    .Select(answer => new GenericTestTakingAnswerTextOnlyDto(
                        answer.Id, answer.OrderInQuestion, (answer.AdditionalInfo as TextOnlyAnswerAdditionalInfo).Text))
                    .OrderBy(dto => dto.OrderInQuestion)
                    .ToArray(),

                AnswersType.ImageOnly => answers
                    .Select(answer => new GenericTestTakingAnswerImageOnlyDto(
                        answer.Id, answer.OrderInQuestion, (answer.AdditionalInfo as ImageOnlyAnswerAdditionalInfo).ImagePath))
                    .OrderBy(dto => dto.OrderInQuestion)
                    .ToArray(),

                AnswersType.TextAndImage => answers
                    .Select(answer => new GenericTestTakingAnswerTextAndImageDto(
                        answer.Id,
                        answer.OrderInQuestion,
                        (answer.AdditionalInfo as TextAndImageAnswerAdditionalInfo).Text,
                        (answer.AdditionalInfo as TextAndImageAnswerAdditionalInfo).ImagePath))
                    .OrderBy(dto => dto.OrderInQuestion)
                    .ToArray(),

                _ => throw new ArgumentException($"Unsupported type {nameof(type)}")
            };
    }
    internal abstract record class GenericTestTakingAnswerDto(GenericTestAnswerId Id, ushort OrderInQuestion);
    internal record class GenericTestTakingAnswerTextOnlyDto(GenericTestAnswerId Id, ushort OrderInQuestion, string Text)
        : GenericTestTakingAnswerDto(Id, OrderInQuestion);
    internal record class GenericTestTakingAnswerImageOnlyDto(GenericTestAnswerId Id, ushort OrderInQuestion, string Image)
        : GenericTestTakingAnswerDto(Id, OrderInQuestion);
    internal record class GenericTestTakingAnswerTextAndImageDto(GenericTestAnswerId Id, ushort OrderInQuestion, string Text, string Image)
        : GenericTestTakingAnswerDto(Id, OrderInQuestion);

}
