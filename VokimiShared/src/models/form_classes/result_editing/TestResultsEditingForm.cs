using VokimiShared.src.models.db_classes.test_creation;

namespace VokimiShared.src.models.form_classes.result_editing
{             
    public class TestResultsEditingForm
    {                     
        public List<ResultWithSaveIdForm> SavedResults { get; init; }
        public List<NotSavedResultForm> NotSavedResults { get; set; }
        public void AddNewResult() =>
            NotSavedResults.Add(NotSavedResultForm.Empty);

        public static TestResultsEditingForm FromDraftTest(BaseDraftTest test) => new() {
            SavedResults = test.PossibleResults
                .Select(ResultWithSaveIdForm.FromDraftTestResult)
                .ToList(),
            NotSavedResults = []
        };
        public static TestResultsEditingForm Empty => new() {
            SavedResults = [],
            NotSavedResults = []
        };
    }
}
