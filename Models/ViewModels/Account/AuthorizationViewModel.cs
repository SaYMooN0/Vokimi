using System.ComponentModel.DataAnnotations;

namespace Vokimi.Models.ViewModels.Account
{
    public class AuthorizationViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
