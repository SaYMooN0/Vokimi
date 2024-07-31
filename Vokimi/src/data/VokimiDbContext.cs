using Microsoft.EntityFrameworkCore;
using Vokimi.src.data.context_configuration.db_entities_relations_classes;
using Vokimi.src.data.context_configuration.model_builder_extensions;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_questions;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
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
        public DbSet<GenericTestAnswer> GenericTestAnswers { get; set; }
        public DbSet<GenericTestResult> GenericTestResults { get; set; }

        //tags
        public DbSet<TestTag> TestTags{ get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            //users
            modelBuilder.ConfigureAppUser();
            modelBuilder.ConfigureLoginInfo();
            modelBuilder.ConfigureUserAdditionalInfo();
            modelBuilder.ConfigureUnconfirmedAppUser();

            //draft tests shared
            modelBuilder.ConfigureBaseDraftTest();
            modelBuilder.ConfigureDraftTestMainInfo();
            modelBuilder.ConfigureDraftTestResult();
            modelBuilder.ConfigureDraftTestTypeSpecificResultData();


            //published and draft tests shared
            modelBuilder.ConfigureTestConclusion();
            modelBuilder.ConfigureTestStylesSheet();

            modelBuilder.ConfigureAnswerTypeSpecificInfo(); //generic test only

            //draft generic test
            modelBuilder.ConfigureDraftGenericTest();
            modelBuilder.ConfigureDraftGenericTestQuestion();
            modelBuilder.ConfigureDraftGenericTestAnswer();

            //published tests
            modelBuilder.ConfigureBaseTest();
            modelBuilder.ConfigureMultiChoiceQuestionData();
            modelBuilder.ConfigureTestTag();

            //published generic test
            modelBuilder.ConfigureTestGenericType();
            modelBuilder.ConfigureGenericTestQuestions();
            modelBuilder.ConfigureGenericTestAnswers();
            modelBuilder.ConfigureGenericTestResults();
        }


    }
}
