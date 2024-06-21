using VokimiShared.src.enums;
using VokimiShared.src.models.dtos.draft_tests;

namespace VokimiShared.src.models.form_classes
{
    public class TestMainInfoEditingForm
    {
        public string Name { get; set; }
        public string? Description { get; set; }
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
        public Err Validate() {
            Err nameErr = ValidateTestName(Name);

            if (nameErr.NotNone())
                return nameErr;
            else if (string.IsNullOrEmpty(Description))
                return Err.None;
            else
                return Description.Length > 510 ? new Err("Length of the test description cannot be more than 510 characters") : Err.None;
        }
        public static TestMainInfoEditingForm FromTestMainInfoDto(DraftTestMainInfoDto dto) =>
            new() {
                Name = dto.Name,
                Description = dto.Description,
                Language = dto.Language,
                Privacy = dto.Privacy
            };
    }
}
