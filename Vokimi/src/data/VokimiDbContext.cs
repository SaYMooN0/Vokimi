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
        public DbSet<LoginInfo> LoginInfo { get; set; }
        public DbSet<UserAdditionalInfo> UserAdditionalInfo { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new(v));
                entity.Property(x => x.LoginInfoId).HasConversion(v => v.Value, v => new(v));
                entity.Property(x => x.UserAdditionalInfoId).HasConversion(v => v.Value, v => new(v));
            });

            modelBuilder.Entity<LoginInfo>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new LoginInfoId(v));
            });
            modelBuilder.Entity<UserAdditionalInfo>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));
            });
            modelBuilder.Entity<UnconfirmedAppUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UnconfirmedAppUserId(v));
            });



        }
    }
}


