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
    }
}
