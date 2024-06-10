using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.users;

namespace Vokimi.src.data
{
    public class VokimiDbContext : DbContext
    {
        public VokimiDbContext(DbContextOptions<VokimiDbContext> options) : base(options) { }
        public DbSet<UnconfirmedAppUser> UnconfirmedAppUsers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<LoginInfo> UserLoginInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UserId(v));
            });

            modelBuilder.Entity<LoginInfo>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new LoginInfoId(v));
            });
            modelBuilder.Entity<UnconfirmedAppUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UnconfirmedAppUserId(v));
            });


        }
    }
}


