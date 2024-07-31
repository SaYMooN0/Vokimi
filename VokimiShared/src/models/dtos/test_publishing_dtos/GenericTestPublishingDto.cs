using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;

namespace Vokimi.src.data.test_publishing_dtos
{
    public record GenericTestPublishingDto(
        BaseTestInfoPublishingDto MainInfo,
        ICollection<GenericTestQuestion> Questions,
        ICollection<GenericTestResult> PossibleResults
        )
    {
        public static GenericTestPublishingDto Create(DraftGenericTest test,
            ICollection<GenericTestQuestion> Questions,
            ICollection<GenericTestResult> PossibleResults) =>

            new(
                BaseTestInfoPublishingDto.FromDraftTest(test),
                Questions,
                PossibleResults
            );

    }
}
