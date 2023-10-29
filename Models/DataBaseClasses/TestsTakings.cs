namespace Vokimi.Models.DataBaseClasses
{
    public class TestsTakings
    {
        public uint Id { get; private set; }
        public uint UserId { get; private set; }
        public uint TestId { get; private set; }
        public int ResultPoints { get; private set; }
        public DateOnly TakingDate { get; private set; }
    }
}
