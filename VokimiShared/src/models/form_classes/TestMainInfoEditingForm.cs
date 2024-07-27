using System.Security;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.dtos.draft_tests;

namespace VokimiShared.src.models.form_classes
{
    public class TestMainInfoEditingForm
    {
        public DraftTestMainInfoId MainInfoId { get; init; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Language Language { get; set; }
        public TestPrivacy Privacy { get; set; }
        public static Err ValidateTestName(string name) {
            if (name.Length > BaseTestCreationConsts.MaxTestNameLength) {
                return new Err($"Length of the test name cannot be more than {BaseTestCreationConsts.MaxTestNameLength} characters");
            }
            else if (name.Length < BaseTestCreationConsts.MinTestNameLength) {
                return new Err($"Length of the test name cannot be less than {BaseTestCreationConsts.MinTestNameLength} characters");
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
                return Description.Length > BaseTestCreationConsts.MaxTestDescriptionLength ?
    new($"Length of the test description cannot be more than {BaseTestCreationConsts.MaxTestDescriptionLength} characters")
                    : Err.None;
        }
        public static TestMainInfoEditingForm FromDraftTestMainInfo(DraftTestMainInfo mainInfo) =>
            new() {
                MainInfoId = mainInfo.Id,
                Name = mainInfo.Name,
                Description = mainInfo.Description,
                Language = mainInfo.Language,
                Privacy = mainInfo.Privacy
            };
        public static TestMainInfoEditingForm Empty() => new() {
            MainInfoId = new(Guid.Empty),
            Name = string.Empty,
            Description = string.Empty,
            Language = Language.Eng,
            Privacy = TestPrivacy.Anyone
        };
    }
}
