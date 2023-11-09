namespace Vokimi.Models.ViewModels.Account
{
    public class MyAccountViewModel : UserProfileViewModel
    {
        public string Email { get; set; }
        public List<int> TakenTests { get; set; } = new();
        public List<int> RatedTests { get; set; } = new();
        public List<int> PinnedTests { get; set; } = new();
        public MyAccountViewModel(int id, string nickname, string email, string? status) : base(id, nickname, status)
        {
            Email = email;
        }
    }
}
