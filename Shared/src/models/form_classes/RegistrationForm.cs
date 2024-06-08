using System.Text.RegularExpressions;

namespace Shared.src.models.form_classes
{
    public class RegistrationForm
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Err Err { get; set; }
        public RegistrationForm(string userName, string email, string password, string confirmPassword, Err err)
        {
            UserName = userName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Err = err;
        }
        public static RegistrationForm Empty() =>
            new("", "", "", "", Err.None);

        public bool AnyError() => Err != Err.None;
        public Err IsValid()
        {
            if (!ValidateEmail())
            {
                return new Err("Email is invalid.");
            }
            if (Password != ConfirmPassword)
            {
                return new Err("Passwords do not match.");
            }
            return Err.None;
        }
        private bool ValidateEmail()
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(Email);
        }

    }
}
