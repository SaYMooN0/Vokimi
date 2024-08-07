using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
using VokimiShared.src.models.dtos;

namespace VokimiShared.src.models.db_classes.test.test_types
{
    public class TestGenericType : BaseTest
    {
        public override TestTemplate Template => TestTemplate.Generic;

        public virtual List<GenericTestQuestion> Questions { get; init; } = [];
        public virtual ICollection<GenericTestResult> PossibleResults { get; init; } = [];
        public static TestGenericType CreateNew(TestPublishingDto dto,
            List<GenericTestQuestion> questions,
            ICollection<GenericTestResult> possibleResults) =>
            new() {
                Id = dto.Id,
                CreatorId = dto.CreatorId,
                Name = dto.Name,
                Cover = dto.NewCover,
                Description = dto.Description,
                Language = dto.Language,
                Privacy = dto.Privacy,
                CreationDate = dto.CreationDate,
                PublicationDate = DateTime.UtcNow,
                ConclusionId = dto.ConclusionId,
                StylesSheetId = dto.StylesSheetId,
                Questions = questions,
                PossibleResults = possibleResults
            };

    }
}
