
using VokimiShared.src.models.db_classes.tests;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public interface IDraftTest
    {
        public DraftTestId Id { get; init; }
        public DraftTestMainInfoId MainInfoId { get; init; }
        public abstract DraftTestMainInfo MainInfo { get; protected set; }
        public DateTime CreationDate { get; init; }
    }
}
