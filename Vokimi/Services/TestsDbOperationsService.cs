using Vokimi.src.data;

namespace Vokimi.Services
{
    public class TestsDbOperationsService
    {
        private readonly VokimiDbContext _context;

        public TestsDbOperationsService(VokimiDbContext context)
        {
            _context = context;
        }
    }
}
