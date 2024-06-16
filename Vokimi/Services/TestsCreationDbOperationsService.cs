using OneOf;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace Vokimi.Services
{
    public class TestsCreationDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TestsCreationDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public async Task<OneOf<DraftTestId, Err>> CreateNewDraftTest(string testName, AppUser creator) {
            DraftTestMainInfo mainInfo = DraftTestMainInfo.CreateNew(testName);
            DraftGenericTest test = DraftGenericTest.CreateNew(creator.Id, mainInfo.Id);

            try {
                await _db.DraftTestMainInfo.AddAsync(mainInfo);
                await _db.SaveChangesAsync();

                await _db.DraftGenericTests.AddAsync(test);
                _db.SaveChanges();

            } catch (Exception ex) { return new Err(ex); }
            return test.Id;
        }
    }
}
