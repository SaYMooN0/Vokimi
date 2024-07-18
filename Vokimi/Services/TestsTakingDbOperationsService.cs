using Vokimi.src.data;

namespace Vokimi.Services
{
    public class TestsTakingDbOperationsService
    {
        private readonly VokimiDbContext _context;

        public TestsTakingDbOperationsService(VokimiDbContext context)
        {
            _context = context;
        }
    }
}
