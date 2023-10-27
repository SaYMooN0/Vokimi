namespace Vokimi.Models.DataBaseClasses
{
    public class Test
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public uint AuthorId { get; private set; }
        public AgeRestriction AgeRestriction { get; private set; }
    }
}
