using Microsoft.EntityFrameworkCore;
using Vokimi.src.data;
using VokimiShared.src;
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
        public async Task<List<Err>> CheckDraftTestForErrors(DraftTestId id) {
            BaseDraftTest? test = await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id);
            if (test is null) {
                return [new("Unknown test")];
            }
            List<Err> errs = [];
            //main info

            //questions
            //min questions count for test, for each question answers

            //results
            //check imgs and text

            //styles
            return errs;
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
