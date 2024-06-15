using VokimiShared.src.enums;

namespace VokimiShared.src.models.db_classes.tests
{
    public class DraftTestMainInfo
    {
        public DraftTestMainInfoId Id { get; init; }
        public string Name { get; init; }
        public string CoverImagePath { get; init; }
        public string? Description { get; init; }
        public Language Language { get; init; }
        public TestPrivacy Privacy { get; init; }





    }

}
