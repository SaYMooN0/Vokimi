using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models
{
    public class TestCreationData
    {
        public string TestName { get; set; } = "";
        public string? Description { get; set; }
        public AgeRestriction AgeRestriction { get; set; } = AgeRestriction.AllAges;
        public Language Language { get; set; } = Language.Unset;
        public List<Question> Questions { get; set; } = new();
        public List<Result> Results { get; set; } = new();
    }
}
