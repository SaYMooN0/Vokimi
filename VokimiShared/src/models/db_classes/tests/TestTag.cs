namespace VokimiShared.src.models.db_classes.tests
{
    public class TestTag
    {
        public TestTagId Id { get; init; }
        public string Value { get; init; }
        public override string ToString() => Value;
    }
}
