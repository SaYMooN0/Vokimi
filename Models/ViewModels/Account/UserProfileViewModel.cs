namespace Vokimi.Models.ViewModels.Account
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string? Status { get; set; }
        public int? LastTakenTest { get; set; }
        public List<int> CreatedTests { get; set; }
        public UserProfileViewModel(int id, string nickname, string? status)
        {
            Id = id;
            Nickname = nickname;
            Status = status == null ? "" : status;
        }
    }
}
