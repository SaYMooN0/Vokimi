using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public class DraftGenericTest : IDraftTest
    {
        public DraftTestId Id { get; init; }
        public virtual DraftTestMainInfoId MainInfoId { get; init; }
        public virtual DraftTestMainInfo MainInfo { get; set; }
        public DateTime CreationDate { get; init; }
        //Questions 
        //results


        public HashSet<TestTag> Tags { get; init; } = new();
        public TestConclusion? Conclusion { get; init; }
    }
}
