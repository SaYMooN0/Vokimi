using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_classes.answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;

namespace Vokimi.src.data
{
    public class VokimiDbContext : DbContext
    {
        public VokimiDbContext(DbContextOptions<VokimiDbContext> options) : base(options) { }

        //users
        public DbSet<UnconfirmedAppUser> UnconfirmedAppUsers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<LoginInfo> LoginInfo { get; set; }
        public DbSet<UserAdditionalInfo> UserAdditionalInfo { get; set; }
        //draft tests only
        public DbSet<BaseDraftTest> DraftTestsSharedInfo { get; set; }
        public DbSet<DraftGenericTest> DraftGenericTests { get; set; }
        public DbSet<DraftTestMainInfo> DraftTestMainInfo { get; set; }
        public DbSet<DraftTestQuestion> DraftTestQuestions { get; set; }
        public DbSet<DraftTestAnswer> DraftTestAnswers { get; set; }
        public DbSet<DraftTestResult> DraftTestResults { get; set; }
        //published and draft tests shared
        public DbSet<TestConclusion> TestConclusions { get; set; }
        public DbSet<TestStylesSheet> TestStyles { get; set; }
        public DbSet<AnswerTypeSpecificInfo> AnswerTypeSpecificInfo { get; set; }
        public DbSet<TextOnlyAnswerAdditionalInfo> AnswerAdditionalForInfoImageOnly { get; set; }
        public DbSet<TextAndImageAnswerAdditionalInfo> AnswerAdditionalForTextAndImage { get; set; }
        public DbSet<ImageOnlyAnswerAdditionalInfo> AnswerAdditionalForTextOnly { get; set; }


        //published tests only
        public DbSet<BaseTest> TestsSharedInfo { get; set; }
        public DbSet<TestGenericType> TestsGenericType { get; set; }

        //tags
        public DbSet<TestTag> TestTags{ get; set; }
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

                entity.HasOne(x => x.StylesSheet)
                     .WithMany()
                     .HasForeignKey(x => x.StylesSheetId);
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
            modelBuilder.Entity<DraftTestAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new DraftTestAnswerId(v));
                entity.Property(e => e.QuestionId).IsRequired();
                entity.Property(e => e.OrderInQuestion).IsRequired();

                entity.HasMany(e => e.RelatedResults)
                      .WithMany(r => r.AnswersLeadingToResult)
                      .UsingEntity<Dictionary<string, object>>(
                          "DraftTestAnswerResultRelations",
                          r => r.HasOne<DraftTestResult>().WithMany().HasForeignKey("DraftTestResultId"),
                          l => l.HasOne<DraftTestAnswer>().WithMany().HasForeignKey("DraftTestAnswerId")
                      );

                entity.HasOne(e => e.AdditionalInfo)
                      .WithOne()
                      .HasForeignKey<DraftTestAnswer>(e => e.AdditionalInfoId);
            });

            modelBuilder.Entity<AnswerTypeSpecificInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new AnswerTypeSpecificInfoId(v));
            });

            modelBuilder.Entity<ImageOnlyAnswerAdditionalInfo>().ToTable("AnswerAdditionalForInfoImageOnly");
            modelBuilder.Entity<TextAndImageAnswerAdditionalInfo>().ToTable("AnswerAdditionalForTextAndImage");
            modelBuilder.Entity<TextOnlyAnswerAdditionalInfo>().ToTable("AnswerAdditionalForTextOnly");

            modelBuilder.Entity<DraftTestResult>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestResultId(v));
                entity.Property(x => x.StringId).IsRequired();
                entity.Property(x => x.Text).IsRequired();
                entity.Property(x => x.ImagePath).IsRequired(false);
            });
            modelBuilder.Entity<TestStylesSheet>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestStylesSheetId(v));
            });

            modelBuilder.Entity<BaseTest>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestId(v));
                entity.Property(x => x.CreatorId).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Language).IsRequired();
                entity.Property(x => x.Privacy).IsRequired();
                entity.Property(x => x.CreationDate).IsRequired();
                entity.Property(x => x.PublicationDate).IsRequired();
                entity.Property(x => x.ConclusionId).HasConversion(
                    v => v.HasValue ? v.Value.Value : (Guid?)null,
                    v => v.HasValue ? new TestConclusionId(v.Value) : null);
                entity.Property(x => x.StylesSheetId).HasConversion(v => v.Value, v => new TestStylesSheetId(v));

                //entity.HasOne(x => x.Conclusion)
                //      .WithMany()
                //      .HasForeignKey(x => x.ConclusionId);

                //entity.HasOne(x => x.StylesSheet)
                //      .WithMany()
                //      .HasForeignKey(x => x.StylesSheetId);


                entity.HasMany(x => x.Tags)
                      .WithMany(x => x.Tests)
                      .UsingEntity<Dictionary<string, object>>(
                          "TestWithTagRelations",
                          j => j.HasOne<TestTag>().WithMany().HasForeignKey("TagId"),
                          j => j.HasOne<BaseTest>().WithMany().HasForeignKey("TestId")
                      );
            });
            modelBuilder.Entity<TestGenericType>(entity =>
            {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.TestId);

            });
            modelBuilder.Entity<TestTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new TestTagId(v));
                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}
