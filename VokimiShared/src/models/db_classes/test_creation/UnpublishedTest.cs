using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class UnpublishedTest
    {
        public UnpublishedTestId Id { get; init; }
        public string Name { get; init; }
        public string CoverImagePath { get; init; }
        public string? Description { get; init; }
        public Language Language { get; init; }
        public TestPrivacy Privacy { get; init; }
        public HashSet<TestTag> Tags { get; init; } = new();
        public TestConclusion? Conclusion { get; init; }
        public DateTime CreationDate { get; init; }



        //Questions 
        //results

    }

}
