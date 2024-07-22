using Microsoft.EntityFrameworkCore;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.constants_store_classes;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;

namespace Vokimi.Services
{
    public class TestsPublishingDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TestsPublishingDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public async Task<List<string>> CheckDraftTestForProblems(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) {
                return ["Unable to find the test"];
            }
            List<string> problems = await CheckTestMainInfoForProblems(test.MainInfo);

            //problems.AddRange(...);

            //questions
            //min questions count for test, for each question answers

            //results
            //check imgs and text

            //styles
            return problems;
        }
        private async Task<List<string>> CheckTestMainInfoForProblems(DraftTestMainInfo mainInfo) {
            List<string> problems = [];
            string errPrefix = "Test main info err:";
            if (
                string.IsNullOrWhiteSpace(mainInfo.Name) ||
                mainInfo.Name.Length > TestCreationConsts.MaxTestNameLength ||
                mainInfo.Name.Length < TestCreationConsts.MinTestNameLength) {
                problems.Add($"{errPrefix} The name of the test must be from {TestCreationConsts.MinTestNameLength} to {TestCreationConsts.MaxTestNameLength} characters");
            }
            if (!string.IsNullOrEmpty(mainInfo.Description) && mainInfo.Description.Length > TestCreationConsts.MaxTestDescriptionLength) {
                problems.Add($"{errPrefix} The description of the test cannot be more than {TestCreationConsts.MaxTestDescriptionLength} characters");
            }
            return problems;
        }
        public async Task<Err> PublishDraftTest(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) {
                return new("Unknown test");
            }
            if (test.Template == TestTemplate.Generic && test is DraftGenericTest genericTest)
                return await PublishGenericDraftTest(genericTest);
            else
                throw new NotImplementedException();
        }
        private async Task<Err> PublishGenericDraftTest(DraftGenericTest test) {
            throw new NotImplementedException();
        }
    }
}
