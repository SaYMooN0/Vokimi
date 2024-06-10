using System.ComponentModel.DataAnnotations;

namespace VokimiShared.src.models.form_classes
{
    public class RegistrationForm
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 30 characters long")]
        [RegularExpression(@"^[a-zA-Z0-9_.,><~^А-Яа-яЁё]*$", ErrorMessage = "Username contains invalid characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }


}
