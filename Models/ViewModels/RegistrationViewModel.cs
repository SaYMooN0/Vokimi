using System.ComponentModel.DataAnnotations;

namespace Vokimi.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel()
        {
            Nickname = string.Empty;
            BirthDate = DateOnly.FromDateTime( DateTime.Now);
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
        }

        [Required]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsValid()
        {
            ErrorMessage= string.Empty;

            return false;
        }
        public override string ToString()
        {
            return $"{{{Nickname}, {BirthDate}, {Email}, {Password}}}";
        }
    }
}
