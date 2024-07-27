using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.db_entities_relations_classes
{
    public class TestWithTagRelations
    {
        public TestId TestId { get; set; }
        public virtual BaseTest Test { get; set; }

        public TestTagId TagId { get; set; }
        public virtual TestTag Tag { get; set; }
    }
}
