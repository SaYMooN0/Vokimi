using VokimiShared.src.models.db_classes.users;
using Microsoft.EntityFrameworkCore;
using VokimiShared.src;
using VokimiShared.src.models.form_classes;
using OneOf;

namespace Vokimi.src.data.db_operations
{
    internal static class UsersDbOperations
    {
        internal static async Task<bool> IsEmailTaken(VokimiDbContext db, string email) =>
            await db.LoginInfo.AnyAsync(i => i.Email == email);

        internal static async Task<OneOf<UnconfirmedAppUser, Err>> CreateUnconfirmedUser(VokimiDbContext db, RegistrationForm data, string confirmationCode) {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(data.Password);
            UnconfirmedAppUser user = UnconfirmedAppUser.CreateNew(data.Username, data.Email, passwordHash, confirmationCode);

            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    var existingUser = await db.UnconfirmedAppUsers.FirstOrDefaultAsync(u => u.Email == data.Email);
                    if (existingUser != null) {
                        db.UnconfirmedAppUsers.Remove(existingUser);
                    }

                    await db.UnconfirmedAppUsers.AddAsync(user);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err(ex);
                }
                return user;
            }

        }

        internal static async Task<OneOf<AppUser, Err>> TryToConfirmUser(VokimiDbContext db, string email, string confirmationCode) {
            UnconfirmedAppUser? unconfirmed = await db.UnconfirmedAppUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (unconfirmed is null || confirmationCode != unconfirmed.ConfirmationCode)
                return new Err("Incorrect confirmation link");
            var loginInfo = LoginInfo.CreateNew(unconfirmed.Email, unconfirmed.PasswordHash);
            var additionalInfo = UserAdditionalInfo.CreateNew(unconfirmed.RegistrationDate);
            var user = AppUser.CreateNew(unconfirmed.Username, loginInfo.Id, additionalInfo.Id);
            using (var transaction = await db.Database.BeginTransactionAsync()) {
                try {
                    db.UserAdditionalInfo.Add(additionalInfo);
                    db.LoginInfo.Add(loginInfo);
                    db.AppUsers.Add(user);
                    db.UnconfirmedAppUsers.Remove(unconfirmed);
                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return user;
                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    return new Err("Server error. Please try again later");
                }
            }

        }
    }
}
