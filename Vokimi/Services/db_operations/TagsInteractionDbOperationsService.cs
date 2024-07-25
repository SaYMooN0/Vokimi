using OneOf;
using Vokimi.src.data;
using VokimiShared.src;

namespace Vokimi.Services.db_operations
{
    public class TagsInteractionDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public TagsInteractionDbOperationsService(VokimiDbContext context) {
            _db = context;
        }
        public async Task<OneOf<List<string>,Err>> GetRelevantTags(string tag) {
            List<string> tags = [];
            return tags;
        }
    }
}
