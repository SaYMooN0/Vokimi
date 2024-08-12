using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.Helpers
{
    public class TestTakingHelper
    {
        public static void SaveSelectedAnswersToCookie(IHttpContextAccessor httpContextAccessor, Guid testId, Guid questionId, IEnumerable<GenericTestAnswerId> selectedAnswers) {
        }

        public static HashSet<GenericTestAnswerId> LoadSelectedAnswersFromCookie(IHttpContextAccessor httpContextAccessor, Guid testId, Guid questionId) {

            return new HashSet<GenericTestAnswerId>();
        }
    }
}
