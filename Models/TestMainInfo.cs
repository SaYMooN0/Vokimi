namespace Vokimi.Models
{
    public class TestMainInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool IsPinned { get; set; }
        public List<string> Tags { get; set; } = new();
        public int QuestionsCount { get; set; }
        public int CommentsCount { get; set; }
        public int TakingsCount { get; set; }
        public int AverageRating { get; set; }
        public TestMainInfo(int id, string name, string? imagePath, int questionsCount, int commentsCount, int takingsCount, int averageRating)
        {
            Id = id;
            Name = name;
            ImagePath = imagePath ?? "default";
            QuestionsCount = questionsCount;
            CommentsCount = commentsCount;
            TakingsCount = takingsCount;
            AverageRating = averageRating;
        }
    }
}
