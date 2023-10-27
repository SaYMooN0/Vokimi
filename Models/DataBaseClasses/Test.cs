namespace Vokimi.Models.DataBaseClasses
{
    public class Test
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public uint AuthorId { get; private set; }
        public AgeRestriction AgeRestriction { get; private set; }
        public Language Language { get; private set; }
        public DateTime CreationTime { get; private set; }
        public List<uint> CommentsId { get; private set; } = new();
        public List<uint> TakingsId { get; private set; } = new();
        public List<uint> RatingsId { get; private set; } = new();
        public List<uint> QuestionsId { get; private set; } = new();
        public List<uint> ResultsId { get; private set; } = new();
        public HashSet<TestTag> Tags { get; private set; } = new();
    }
}
