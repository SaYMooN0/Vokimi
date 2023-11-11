using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.Tests
{
    public class TestViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; } = "No description";
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public HashSet<string> Tags { get; set; } = new();
        public Double AverageRating { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public TestViewModel()
        {
        }
        public TestViewModel(Test test, string author)
        {
            Id = test.Id;
            TestName = test.Name;
            ImagePath = test.ImagePath ?? "default";
            Description = test.Description ?? "";
            Author = author;
            AuthorId = test.AuthorId;
            Tags= test.Tags.Select(tag => tag.ToString()).ToHashSet();
            AverageRating = test.AverageRating();
            Comments = test.Comments;
        }

    }
}
