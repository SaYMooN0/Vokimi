using OneOf;
using VokimiShared.src;
using VokimiShared.src.models.db_classes.tests;

namespace Vokimi.src.data.db_operations
{
    static internal class TagsInteractionsDbOperations
    {
        private static readonly Err TagCannotBeEmptyErr = new Err("Tag cannot be empty");

        internal static OneOf<List<string>, Err> GetRelevantTags(VokimiDbContext db, string tag) {
            if (string.IsNullOrWhiteSpace(tag)) {
                return TagCannotBeEmptyErr;
            }
            else {
                return db.TestTags
                    .Where(t => t.Value.Contains(tag))
                    .Select(t => t.Value)
                    .ToList();
            }
        }
        internal static async Task<Err> CreateNewTag(VokimiDbContext db, string tag) {
            if (string.IsNullOrWhiteSpace(tag)) {
                return TagCannotBeEmptyErr;
            }
            else {
                try {
                    TestTag dbTag = TestTag.CreateNew(tag);
                    db.TestTags.Add(dbTag);
                    await db.SaveChangesAsync();
                    return Err.None;
                } catch (Exception ex) {
                    return new Err("Unable to create tag");
                }
            }
        }
    }
}
