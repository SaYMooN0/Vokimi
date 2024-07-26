﻿using Vokimi.src.data;
using VokimiShared.src.models.db_classes.users;
using Microsoft.EntityFrameworkCore;
using VokimiShared.src;
using VokimiShared.src.models.form_classes;
using OneOf;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.Services.db_operations
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
            UnconfirmedAppUser user = UnconfirmedAppUser.CreateNew(data.Username, data.Email, passwordHash, confirmationCode);
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

        public async Task<OneOf<AppUser, Err>> TryToConfirmUser(string email, string confirmationCode)
        {
            UnconfirmedAppUser? unconfirmed = _db.UnconfirmedAppUsers.FirstOrDefault(u => u.Email == email);
            if (unconfirmed is null)
                return new Err("Incorrect confirmation link");
            if (confirmationCode != unconfirmed.ConfirmationCode)
                return new Err("Incorrect confirmation link");


            var loginInfo = LoginInfo.CreateNew(unconfirmed.Email, unconfirmed.PasswordHash);
            var additionalInfo = UserAdditionalInfo.CreateNew(unconfirmed.RegistrationDate);
            var user = AppUser.CreateNew(unconfirmed.Username, loginInfo.Id, additionalInfo.Id);

            try
            {
                _db.UserAdditionalInfo.Add(additionalInfo);
                _db.LoginInfo.Add(loginInfo);
                _db.AppUsers.Add(user);
                _db.SaveChanges();
            }
            catch { return new Err("Server error. Please try again later"); }

            await RemoveUnconfirmedAppUser(email);
            return user;
        }
        public async Task<AppUser?> GetUserByEmail(string email) =>
            await _db.AppUsers.FirstOrDefaultAsync(u => u.LoginInfo.Email == email);
        public async Task<AppUser?> GetUserById(AppUserId id) =>
            await _db.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
    }
}
