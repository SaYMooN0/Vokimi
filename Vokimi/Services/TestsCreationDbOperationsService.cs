using Microsoft.EntityFrameworkCore;
using OneOf;
using Vokimi.src.data;
using VokimiShared.src;
using VokimiShared.src.enums;
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
        public async Task<OneOf<DraftTestId, Err>> CreateNewDraftTest(string testName, TestTemplate template, AppUser creator) {
            DraftTestMainInfo mainInfo = DraftTestMainInfo.CreateNewFromName(testName);
            DraftGenericTest test = DraftGenericTest.CreateNew(creator.Id, mainInfo.Id);

            try {
                await _db.DraftTestMainInfo.AddAsync(mainInfo);
                await _db.SaveChangesAsync();

                switch (template) {
                    case TestTemplate.Generic:
                        await _db.DraftGenericTests.AddAsync(test);
                        break;
                    case TestTemplate.Knowledge:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentException("Invalid template type");
                }
                await _db.SaveChangesAsync();

            } catch (Exception ex) {
                return new Err(ex);
            }
            return test.Id;
        }
        public async Task<TestTemplate?> GetTestTypeById(DraftTestId id) =>
            (await _db.DraftTestsSharedInfo.FirstOrDefaultAsync(i => i.Id == id))?.Template;
        public async Task<T?> GetTestById<T>(DraftTestId id, TestTemplate template) where T : BaseDraftTest =>
            await _db.Set<T>().FirstOrDefaultAsync(i => i.Id == id && i.Template == template);
        public async Task<DraftTestMainInfo?> GetDraftTestMainInfoById(DraftTestMainInfoId id) =>
            await _db.DraftTestMainInfo.FirstOrDefaultAsync(mi => mi.Id == id);
        public async Task<Err> UpdateTestCover(DraftTestMainInfoId mainInfoId, string newPath) {
            var mainInfo = await GetDraftTestMainInfoById(mainInfoId);
            if (mainInfo is null) {
                return new Err("Test cannot be found");
            }
            if (mainInfo.CoverImagePath == newPath) {
                return Err.None;
            }
            mainInfo.UpdateCoverImage(newPath);
            try {
                _db.DraftTestMainInfo.Update(mainInfo);
                await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return new Err("Server error. Please try again later");
            }
            return Err.None;

        }
    }
}
