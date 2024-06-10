using Microsoft.EntityFrameworkCore;

namespace Vokimi.src.data
{
    public class DbInitializer
    {
        public static async Task InitializeDbAsync(VokimiDbContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (dbContext.AppUsers.Any() || dbContext.UnconfirmedAppUsers.Any()) return;


            //adding fake users
            //await dbContext.SaveChangesAsync();
        }
    }
}
