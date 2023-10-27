namespace Vokimi.Models.DataBaseClasses
{
    public class TestsRatings
    {
        public uint Id { get; private set; }
        public uint UserId { get;private set; }
        public uint TestId { get;private set; }
        public ushort Rating { get; private set; }

    }
}
