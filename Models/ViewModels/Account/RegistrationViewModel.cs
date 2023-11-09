using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Vokimi.Models.ViewModels.Account
{
    public class RegistrationViewModel
    {
        public RegistrationViewModel()
        {
            Nickname = string.Empty;
            BirthDate = DateOnly.FromDateTime(DateTime.Now);
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
        }
        public string Nickname { get; set; }

        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string? ErrorMessage { get; set; }
        public bool IsValid()
        {
            ErrorMessage = string.Empty;
            if (!IsValidEmail())
            {
                ErrorMessage = "Invalid email";
                return false;
            }
            else if (Nickname.Length > 40 || Nickname.Length < 5)
            {
                ErrorMessage = "The length of the nickname should be from 5 to 40 characters";
                return false;
            }
            else if (!isVaildNickname())
            {
                ErrorMessage = "Nickname cannot contain the following characters: .,/\\[]{}&`'\"";
                return false;
            }
            else if (Password.Length > 40 || Password.Length < 8)
            {
                ErrorMessage = "The length of password should be from 8 to 40 characters";
                return false;
            }
            else if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords don't match";
                return false;
            }
            if (BirthDate.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)) > DateTime.Now || BirthDate.Year < 1900)
            {
                ErrorMessage = "Incorrect date of birth";
                return false;
            }

            return true;
        }
        public override string ToString()
        {
            return $"{{{Nickname}, {BirthDate}, {Email}, {Password}}}";
        }
        public bool IsValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;
            try
            {
                return Regex.IsMatch(Email, @"^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,6}$");
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private bool isVaildNickname()
        {
            string pattern = @"[.,/\\[\]{}&`'""]";
            return !Regex.IsMatch(Nickname, pattern);
        }
    }
}
