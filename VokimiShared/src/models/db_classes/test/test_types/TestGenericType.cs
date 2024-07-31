using Vokimi.src.data.test_publishing_dtos;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;

namespace VokimiShared.src.models.db_classes.test.test_types
{
    public class TestGenericType : BaseTest
    {
        public override TestTemplate Template => TestTemplate.Generic;

        public virtual ICollection<GenericTestQuestion> Questions { get; init; } = [];
        public virtual ICollection<GenericTestResult> PossibleResults { get; init; } = [];

        public static TestGenericType CreateNewFromPublishingDto(GenericTestPublishingDto dto) => new() {
            Id=new(),
            CreatorId=dto.MainInfo.CreatorId,
            Name=dto.MainInfo.Name,
            Cover=dto.MainInfo.Cover,
            Description=dto.MainInfo.Description,
            Language=dto.MainInfo.Language,
            Privacy=dto.MainInfo.Privacy,
            CreationDate=dto.MainInfo.CreationDate,
            PublicationDate=DateTime.UtcNow,
            ConclusionId=dto.MainInfo.ConclusionId,
            StylesSheetId=dto.MainInfo.StylesSheetId,
            Questions= dto.Questions,
            PossibleResults= dto.PossibleResults
        };

    }
}
