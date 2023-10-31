namespace Vokimi.Models.DataBaseClasses
{
    public class Test
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? ImagePath { get; private set; }
        public string? Description { get; private set; }
        public int AuthorId { get; private set; }
        public AgeRestriction AgeRestriction { get; private set; }
        public Language Language { get; private set; }
        public DateTime CreationTime { get; private set; }
        public List<int> CommentsId { get; private set; } = new();
        public List<int> TakingsId { get; private set; } = new();
        public List<int> RatingsId { get; private set; } = new();
        public List<int> QuestionsId { get; private set; } = new();
        public List<int> ResultsId { get; private set; } = new();
        public HashSet<TestTag> Tags { get; private set; } = new();
    }
}
