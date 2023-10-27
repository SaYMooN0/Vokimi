namespace Vokimi.Models.DataBaseClasses
{
    public class Comment
    {
        public uint Id { get; private set; }
        public uint UserId { get; private set; }
        public uint TestId { get; private set; }
        public int Text { get; private set; }
    }
}
