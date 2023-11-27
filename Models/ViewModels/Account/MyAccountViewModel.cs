using Vokimi.Models.DataBaseClasses;

namespace Vokimi.Models.ViewModels.Account
{
    public class MyAccountViewModel : UserProfileViewModel
    {
        public string Email { get; set; }
        public List<TestMainInfo> TakenTests { get; set; } = new();
        public List<TestMainInfo> RatedTests { get; set; } = new();
        public List<TestMainInfo> PinnedTests { get; set; } = new();
        public MyAccountViewModel(int id, string nickname, string email, string? status) : base(id, nickname, status)
        {
            Email = email;
        }
    }
}
