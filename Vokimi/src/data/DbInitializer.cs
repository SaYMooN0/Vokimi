using VokimiShared.src.models.db_classes.users;

namespace Vokimi.src.data
{
    public class DbInitializer
    {
        public static async Task InitializeDbAsync(VokimiDbContext dbContext)
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            if (dbContext.AppUsers.Any() || dbContext.UnconfirmedAppUsers.Any()) return;

            else
            {
                AddNewFakeUser(dbContext, "email@email.email", "email@email.email", "emailUser");
                AddNewFakeUser(dbContext, "admin@admin.admin", "admin@admin.admin", "adminUser");
                await dbContext.SaveChangesAsync();
            }
        }
        private static void AddNewFakeUser(VokimiDbContext dbContext, string email, string password, string username)
        {
            var loginInfo = LoginInfo.CreateNew(email, BCrypt.Net.BCrypt.HashPassword(password));
            var additionalInfo = UserAdditionalInfo.CreateNew(DateTime.UtcNow);
            var user = AppUser.CreateNew(username, loginInfo.Id, additionalInfo.Id);
            dbContext.UserAdditionalInfo.Add(additionalInfo);
            dbContext.LoginInfo.Add(loginInfo);
            dbContext.AppUsers.Add(user);
        }
    }
}
