using Vokimi.src.data;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_classes;
using Microsoft.EntityFrameworkCore;
using VokimiShared.src;
using VokimiShared.src.models.form_classes;
using OneOf;
using Microsoft.VisualBasic;

namespace Vokimi.Services
{
    public class UsersDbOperationsService
    {
        private readonly VokimiDbContext _db;

        public UsersDbOperationsService(VokimiDbContext context)
        {
            _db = context;
        }
        public async Task<bool> IsEmailTaken(string email) =>
            await _db.LoginInfo.AnyAsync(i => i.Email == email);
        public async Task<OneOf<UnconfirmedAppUser, Err>> CreateUnconfirmedUser(RegistrationForm data, string confirmationCode)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(data.Password);
            UnconfirmedAppUser user = new(
                data.Username,
                data.Email,
                passwordHash,
                confirmationCode,
                DateTime.UtcNow);
            try
            {
                Err removingErr = await RemoveUnconfirmedAppUser(data.Email);
                if (removingErr.NotNone())
                    return removingErr;

                await _db.UnconfirmedAppUsers.AddAsync(user);
                _db.SaveChanges();
            }
            catch (Exception ex) { return new Err(ex); }
            return user;
        }
        public async Task<Err> RemoveUnconfirmedAppUser(string email)
        {
            try
            {
                UnconfirmedAppUser? user = await _db.UnconfirmedAppUsers.FirstOrDefaultAsync(u => u.Email == email);
                if (user is null)
                    return Err.None;
                _db.UnconfirmedAppUsers.Remove(user);
                _db.SaveChanges();

            }
            catch (Exception ex) { return new Err(ex); }
            return Err.None;

        }

        public async Task<Err> TryToConfirmUser(string email, string confirmationCode)
        {
            UnconfirmedAppUser? unconfirmed = _db.UnconfirmedAppUsers.FirstOrDefault(u => u.Email == email);
            if (unconfirmed is null)
                return new("Incorrect confirmation link");
            if (confirmationCode != unconfirmed.ConfirmationCode)
                return new("Incorrect confirmation link");

            LoginInfo loginInfo = new(unconfirmed.Email, unconfirmed.PasswordHash);
            AppUser user = new(unconfirmed.Username, loginInfo.Id);

            try
            {
                _db.LoginInfo.Add(loginInfo);
                _db.AppUsers.Add(user);
                _db.SaveChanges();
            }
            catch { return new("Server error. Please try again later"); }

            await RemoveUnconfirmedAppUser(email);
            return Err.None;
        }
    }
}
