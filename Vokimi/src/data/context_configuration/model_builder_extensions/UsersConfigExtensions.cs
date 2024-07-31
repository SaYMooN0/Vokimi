using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    internal static class UsersConfigExtensions
    {
        internal static void ConfigureAppUser(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<AppUser>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new AppUserId(v));
                entity.Property(x => x.LoginInfoId).HasConversion(v => v.Value, v => new LoginInfoId(v));
                entity.Property(x => x.UserAdditionalInfoId).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));

                entity.HasMany(x => x.DraftTests)
                      .WithOne()
                      .HasForeignKey(x => x.CreatorId);
            });
        }

        internal static void ConfigureLoginInfo(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<LoginInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new LoginInfoId(v));
            });
        }

        internal static void ConfigureUserAdditionalInfo(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserAdditionalInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));
            });
        }

        internal static void ConfigureUnconfirmedAppUser(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<UnconfirmedAppUser>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UnconfirmedAppUserId(v));
            });
        }
    }
}
