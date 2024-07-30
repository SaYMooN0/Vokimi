using OneOf;
using VokimiShared.src;

namespace Vokimi.src.data.db_operations
{
    static internal class TagsInteractionsDbOperations
    {
        internal static OneOf<List<string>, Err> GetRelevantTags(VokimiDbContext db, string tag) {
            if (string.IsNullOrWhiteSpace(tag)) {
                return new Err("Tag cannot be empty");
            }
            else {
                return db.TestTags
                    .Where(t => t.Value.Contains(tag))
                    .Select(t => t.Value)
                    .ToList();
            }
        }
    }
}
