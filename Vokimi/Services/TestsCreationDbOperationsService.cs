using Vokimi.src.data;
using VokimiShared.src;

namespace Vokimi.Services
{
    public class TestsCreationDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TestsCreationDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public Task<Err> CreateNewTestDraft() {
            throw new NotImplementedException();
        }
    }
}
