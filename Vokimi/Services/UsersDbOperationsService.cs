using Vokimi.src.data;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_classes;

namespace Vokimi.Services
{
    public class UsersDbOperationsService
    {
        private readonly VokimiDbContext _context;

        public UsersDbOperationsService(VokimiDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByIdAsync(UserId userId)
        {
            return await _context.AppUsers.FindAsync(userId);
        }

    }
}
