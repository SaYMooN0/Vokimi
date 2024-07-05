using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
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
        public DbSet<BaseDraftTest> DraftTestsSharedInfo { get; set; }
        public DbSet<DraftGenericTest> DraftGenericTests { get; set; }
        public DbSet<DraftTestMainInfo> DraftTestMainInfo { get; set; }
        public DbSet<TestConclusion> TestConclusions { get; set; }
        public DbSet<DraftTestQuestion> DraftTestQuestions { get; set; }
        public DbSet<BaseAnswer> AnswersSharedInfo { get; set; }
        public DbSet<ImageOnlyAnswer> ImageOnlyAnswers { get; set; }
        public DbSet<TextAndImageAnswer> TextAndImageAnswers { get; set; }
        public DbSet<TextOnlyAnswer> TextOnlyAnswers { get; set; }
        public DbSet<DraftTestResult> DraftTestResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new AppUserId(v));
                entity.Property(x => x.LoginInfoId).HasConversion(v => v.Value, v => new LoginInfoId(v));
                entity.Property(x => x.UserAdditionalInfoId).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));

                entity.HasMany(x => x.DraftTests)
                      .WithOne(x => x.Creator)
                      .HasForeignKey(x => x.CreatorId)
                      .IsRequired();
            });

            modelBuilder.Entity<LoginInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new LoginInfoId(v));
            });

            modelBuilder.Entity<UserAdditionalInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));
            });

            modelBuilder.Entity<UnconfirmedAppUser>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UnconfirmedAppUserId(v));
            });

            modelBuilder.Entity<BaseDraftTest>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestId(v));
                entity.Property(x => x.MainInfoId).HasConversion(v => v.Value, v => new DraftTestMainInfoId(v));

                entity.HasOne(x => x.MainInfo)
                      .WithOne()
                      .HasForeignKey<BaseDraftTest>(x => x.MainInfoId);

                entity.HasOne(x => x.Creator)
                      .WithMany(x => x.DraftTests)
                      .HasForeignKey(x => x.CreatorId);

                entity.HasOne(x => x.Conclusion)
                      .WithMany()
                      .HasForeignKey(x => x.ConclusionId);

                entity.HasMany(x => x.PossibleResults)
                      .WithOne()
                      .HasForeignKey(x => x.TestId);
            });

            modelBuilder.Entity<DraftGenericTest>(entity => {

                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.DraftTestId)
                      .IsRequired();
            });

            modelBuilder.Entity<DraftTestMainInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestMainInfoId(v));

            });

            modelBuilder.Entity<TestConclusion>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestConclusionId(v));
            });
            modelBuilder.Entity<DraftTestQuestion>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestQuestionId(v));

                entity.OwnsOne(x => x.MultipleChoiceData, mc => {
                    mc.Property(x => x.MinAnswers).HasColumnName("MinAnswers");
                    mc.Property(x => x.MaxAnswers).HasColumnName("MaxAnswers");
                });

                entity.HasMany(x => x.Answers)
                      .WithOne()
                      .HasForeignKey(x => x.QuestionId);

                entity.Property(x => x.DraftTestId).IsRequired();
            });
            modelBuilder.Entity<BaseAnswer>(entity => {
                entity.HasKey(x => x.AnswerId);
                entity.Property(x => x.AnswerId).HasConversion(v => v.Value, v => new AnswerId(v));

                entity.HasMany(x => x.RelatedResults)
                    .WithMany(r => r.AnswersLeadingToResult)
                    .UsingEntity<Dictionary<string, object>>(
                        "AnswersLeadingToResultsRelations",
                        r => r.HasOne<DraftTestResult>().WithMany().HasForeignKey("DraftTestResultId"),
                        l => l.HasOne<BaseAnswer>().WithMany().HasForeignKey("BaseAnswerId"));
            });

            modelBuilder.Entity<DraftTestResult>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestResultId(v));
                entity.Property(x => x.StringId).IsRequired();
                entity.Property(x => x.Text).IsRequired();
                entity.Property(x => x.ImagePath).IsRequired(false);
            });
        }
    }
}
