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
        public HashSet<TestTag> Tags { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<TestsTaking> Takings { get; set; } = new();
        public List<TestsRating> Ratings { get; set; } = new();
        public List<Question> Questions { get; set; } = new();
        public List<Result> Results { get; set; } = new();
        public Test(string name, string? imagePath, string? description, int authorId, AgeRestriction ageRestriction, Language language, HashSet<TestTag> tags)
        {
            Name = name;
            ImagePath = imagePath;
            Description = description;
            AuthorId = authorId;
            AgeRestriction = ageRestriction;
            Language = language;
            Tags = tags;
            CreationTime = DateTime.Now;
            Comments = new();
            Takings = new();
            Ratings = new();
            Questions = new();
            Results = new();
        }
        public Test(int id, string name, string? description, int authorId, AgeRestriction ageRestriction, Language language, DateTime creationTime, string? imagePath)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath;
            Description = description;
            AuthorId = authorId;
            AgeRestriction = ageRestriction;
            Language = language;
            CreationTime = creationTime;
        }
    }
}
