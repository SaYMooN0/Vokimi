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
        public HashSet<TestTag> Tags { get; private set; } = new();
        public List<int> CommentsId { get; private set; } = new();
        public List<int> TakingsId { get; private set; } = new();
        public List<int> RatingsId { get; private set; } = new();
        public List<int> QuestionsId { get; private set; } = new();
        public List<int> ResultsId { get; private set; } = new();

        public Test(int id, string name, string? imagePath, string? description, int authorId, AgeRestriction ageRestriction, Language language, DateTime creationTime, List<int> commentsId, List<int> takingsId, List<int> ratingsId, List<int> questionsId, List<int> resultsId, HashSet<TestTag> tags)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath;
            Description = description;
            AuthorId = authorId;
            AgeRestriction = ageRestriction;
            Language = language;
            CreationTime = creationTime;
            CommentsId = commentsId;
            TakingsId = takingsId;
            RatingsId = ratingsId;
            QuestionsId = questionsId;
            ResultsId = resultsId;
            Tags = tags;
        }

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
            CommentsId = new();
            TakingsId = new();
            RatingsId = new();
            QuestionsId = new();
            ResultsId = new();

        }
    }
}
