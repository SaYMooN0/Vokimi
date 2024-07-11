using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.form_classes.result_editing
{             
    public class TestResultsEditingForm
    {                     
        public ResultWithSaveIdForm[] SavedResults { get; init; }
        public List<NotSavedResultForm> NotSavedResults { get; set; }
        public static TestResultsEditingForm FromDraftTest(BaseDraftTest test) => new() {
            SavedResults = test.PossibleResults
                .Select(ResultWithSaveIdForm.FromDraftTestResult)
                .ToArray(),
            NotSavedResults = []
        };
        public static TestResultsEditingForm Empty => new() {
            SavedResults = [],
            NotSavedResults = []
        };
    }
}
