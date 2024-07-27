using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.tests
{
    public class TestTag
    {
        public TestTagId Id { get; init; }
        public string Value { get; init; }
        public override string ToString() => Value;
        public virtual ICollection<BaseTest> Tests { get; set; } = [];
    }
}
