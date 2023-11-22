using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.TestTaking
{
    public class TestTakingViewModel
    {
        public int TestId { get; set; }
        public List<Question> Questions { get; set; } = new();

    }
}
