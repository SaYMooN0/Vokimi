using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.dtos.tests
{
    public class TestConclusionDto
    {
        public TestConclusionId Id { get; init; }
        public string Text { get; init; }
        public string? AdditionalImage { get; init; }
        public bool Feedback { get; init; }

        public TestConclusionDto(TestConclusion conclusion) {
            Id = conclusion.Id;
            Text = conclusion.Text;
            AdditionalImage = conclusion.AdditionalImage;
            Feedback = conclusion.Feedback;
        }
    }
}
