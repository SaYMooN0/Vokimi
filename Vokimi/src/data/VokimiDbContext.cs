using Microsoft.EntityFrameworkCore;
using Vokimi.src.data.context_configuration.db_entities_relations_classes;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_questions;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

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
        //draft tests shared
        public DbSet<BaseDraftTest> DraftTestsSharedInfo { get; set; }
        public DbSet<DraftTestMainInfo> DraftTestMainInfo { get; set; }
        public DbSet<DraftTestResult> DraftTestResults { get; set; }
        public DbSet<DraftTestTypeSpecificResultData> DraftTestTypeSpecificResultsData { get; set; }
        //draft generic test
        public DbSet<DraftGenericTest> DraftGenericTests { get; set; }
        public DbSet<DraftGenericTestResultData> DraftGenericTestResultsData { get; set; }
        public DbSet<DraftGenericTestQuestion> DraftGenericTestQuestions { get; set; }
        public DbSet<DraftGenericTestAnswer> DraftGenericTestAnswers { get; set; }
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
        public DbSet<GenericTestQuestion> GenericTestQuestions { get; set; }
        public DbSet<MultiChoiceQuestionData> MultiChoiceQuestionsData { get; set; }

        //tags
        public DbSet<TestTag> TestTags{ get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            ConfigureAppUser(modelBuilder);
            ConfigureLoginInfo(modelBuilder);
            ConfigureUserAdditionalInfo(modelBuilder);
            ConfigureUnconfirmedAppUser(modelBuilder);
            ConfigureBaseDraftTest(modelBuilder);
            ConfigureDraftGenericTest(modelBuilder);
            ConfigureDraftTestMainInfo(modelBuilder);
            ConfigureTestConclusion(modelBuilder);
            ConfigureDraftGenericTestQuestion(modelBuilder);
            ConfigureDraftGenericTestAnswer(modelBuilder);
            ConfigureAnswerTypeSpecificInfo(modelBuilder);
            ConfigureDraftTestResult(modelBuilder);
            ConfigureTestStylesSheet(modelBuilder);
            ConfigureBaseTest(modelBuilder);
            ConfigureTestGenericType(modelBuilder);
            ConfigureGenericTestQuestion(modelBuilder);
            ConfigureMultiChoiceQuestionData(modelBuilder);
            ConfigureTestTag(modelBuilder);
            ConfigureDraftTestTypeSpecificResultData(modelBuilder);
        }

        private void ConfigureAppUser(ModelBuilder modelBuilder) {
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
        }

        private void ConfigureLoginInfo(ModelBuilder modelBuilder) {
            modelBuilder.Entity<LoginInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new LoginInfoId(v));
            });
        }

        private void ConfigureUserAdditionalInfo(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserAdditionalInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UserAdditionalInfoId(v));
            });
        }

        private void ConfigureUnconfirmedAppUser(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UnconfirmedAppUser>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new UnconfirmedAppUserId(v));
            });
        }

        private void ConfigureBaseDraftTest(ModelBuilder modelBuilder) {
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
        }

        private void ConfigureDraftGenericTest(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftGenericTest>(entity => {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.DraftTestId)
                      .IsRequired();
            });
        }

        private void ConfigureDraftTestMainInfo(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestMainInfo>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestMainInfoId(v));
            });
        }

        private void ConfigureTestConclusion(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestConclusion>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestConclusionId(v));
            });
        }

        private void ConfigureDraftGenericTestQuestion(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftGenericTestQuestion>(entity => {
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
        }

        private void ConfigureDraftGenericTestAnswer(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftGenericTestAnswer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new DraftTestAnswerId(v));
                entity.Property(e => e.QuestionId).IsRequired();
                entity.Property(e => e.OrderInQuestion).IsRequired();
                entity.HasOne(e => e.AdditionalInfo)
                      .WithOne()
                      .HasForeignKey<DraftGenericTestAnswer>(e => e.AdditionalInfoId);
                entity.HasMany(e => e.RelatedResults)
                .WithMany()
                .UsingEntity<DraftGenericTestAnswerResultDataRelations>(
                    j => j.HasOne(x => x.DraftTestResultData).WithMany().HasForeignKey(x => x.DraftTestResultDataId),
                    j => j.HasOne(x => x.DraftTestAnswer).WithMany().HasForeignKey(x => x.DraftTestAnswerId)
                );
            });
        }

        private void ConfigureDraftTestResult(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestResult>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestResultId(v));
                entity.Property(x => x.StringId).IsRequired();
                entity.Property(x => x.Text).IsRequired();
                entity.Property(x => x.ImagePath).IsRequired(false);
            });
        }
        private void ConfigureAnswerTypeSpecificInfo(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AnswerTypeSpecificInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new AnswerTypeSpecificInfoId(v));
            });
          


        }
        private void ConfigureDraftTestTypeSpecificResultData(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestTypeSpecificResultData>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new DraftTestTypeSpecificResultDataId(v));
            });
        }
        private void ConfigureTestStylesSheet(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestStylesSheet>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestStylesSheetId(v));
            });
        }

        private void ConfigureBaseTest(ModelBuilder modelBuilder) {
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

                entity.HasOne(x => x.Conclusion)
                      .WithMany()
                      .HasForeignKey(x => x.ConclusionId);

                entity.HasOne(x => x.StylesSheet)
                      .WithMany()
                      .HasForeignKey(x => x.StylesSheetId);

                entity.HasMany(x => x.Tags)
                      .WithMany(x => x.Tests)
                      .UsingEntity<Dictionary<string, object>>(
                          "TestWithTagRelations",
                          j => j.HasOne<TestTag>().WithMany().HasForeignKey("TagId"),
                          j => j.HasOne<BaseTest>().WithMany().HasForeignKey("TestId")
                      );
            });
        }

        private void ConfigureTestGenericType(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestGenericType>(entity =>
            {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.TestId);
            });
        }

        private void ConfigureGenericTestQuestion(ModelBuilder modelBuilder) {
            modelBuilder.Entity<GenericTestQuestion>(entity => {
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new QuestionId(v));
                entity.Property(x => x.MultiChoiceQuestionDataId).HasConversion(
                    v => v.HasValue ? v.Value.Value : (Guid?)null,
                    v => v.HasValue ? new MultiChoiceQuestionDataId(v.Value) : null);
            });
        }

        private void ConfigureMultiChoiceQuestionData(ModelBuilder modelBuilder) {
            modelBuilder.Entity<MultiChoiceQuestionData>(entity => {
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new MultiChoiceQuestionDataId(v));
            });
        }

        private void ConfigureTestTag(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new TestTagId(v));
                entity.Property(e => e.Value).IsRequired();
            });
        }

    }
}
