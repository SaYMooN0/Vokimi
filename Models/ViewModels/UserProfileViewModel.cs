namespace Vokimi.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; } 
        public string Nickname { get; set; }
        public string? Status { get; set; }
        public int? LastTakenTest { get; set; }
        public List<int> CreatedTests { get; set; }
    }
}
