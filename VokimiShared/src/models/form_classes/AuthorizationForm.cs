using System.ComponentModel.DataAnnotations;

namespace VokimiShared.src.models.form_classes
{

    public class AuthorizationForm
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please provide email")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide password")]
        public string Password { get; set; }
    }
}
