using Microsoft.AspNetCore.Http;
using VokimiShared.src.enums;

namespace VokimiShared.src.models.form_classes
{
    public class TestMainInfoEditingForm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Cover { get; set; }
        public Language Language { get; set; }
        public TestPrivacy Privacy { get; set; }
        public static Err ValidateTestName(string name) {
            if (name.Length > 127) {
                return new Err("Length of the test name cannot be more than 127 characters");
            }
            else if (name.Length < 8) {
                return new Err("Length of the test name cannot be less than 8 characters");
            }
            return Err.None;
        }
    }   
}
