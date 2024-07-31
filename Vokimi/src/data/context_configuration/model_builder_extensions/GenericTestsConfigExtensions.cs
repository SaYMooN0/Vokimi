using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_results.results_for_published_tests;
using Vokimi.src.data.context_configuration.db_entities_relations_classes;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    internal static class GenericTestsConfigExtensions
    {
        internal static void ConfigureTestGenericType(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestGenericType>(entity => {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.TestId);
                entity.HasMany(x => x.PossibleResults)
                    .WithOne()
                    .HasForeignKey(x => x.TestId);
            });

        }

        internal static void ConfigureGenericTestQuestions(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<GenericTestQuestion>(entity => {
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new GenericTestQuestionId(v));
                entity.Property(x => x.MultiChoiceQuestionDataId).HasConversion(
                    v => v.HasValue ? v.Value.Value : (Guid?)null,
                    v => v.HasValue ? new MultiChoiceQuestionDataId(v.Value) : null);

                entity.HasMany(q => q.Answers)
                    .WithOne()
                    .HasForeignKey(a => a.QuestionId);
            });

        }
        internal static void ConfigureGenericTestAnswers(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<GenericTestAnswer>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new GenericTestAnswerId(v));

                entity.HasOne(a => a.AdditionalInfo)
                    .WithMany()
                    .HasForeignKey(a => a.AdditionalInfoId);

            });
        }
        internal static void ConfigureAnswerTypeSpecificInfo(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<AnswerTypeSpecificInfo>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new AnswerTypeSpecificInfoId(v));
            });
            modelBuilder.Entity<ImageOnlyAnswerAdditionalInfo>().ToTable("AnswerAdditionalForInfoImageOnly");
            modelBuilder.Entity<TextAndImageAnswerAdditionalInfo>().ToTable("AnswerAdditionalForTextAndImage");
            modelBuilder.Entity<TextOnlyAnswerAdditionalInfo>().ToTable("AnswerAdditionalForTextOnly");
        }
        internal static void ConfigureGenericTestResults(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<GenericTestResult>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new GenericTestResultId(v));

                entity.HasMany(e => e.AnswersLeadingToResult)
                    .WithMany(e => e.RelatedResults)
                    .UsingEntity<GenericTestAnswerLeadingToResultRelations>(
                         j => j.HasOne(pt => pt.GenericTestAnswer).WithMany().HasForeignKey(pt => pt.GenericTestAnswerId),
                         j => j.HasOne(pt => pt.GenericTestResult).WithMany().HasForeignKey(pt => pt.GenericTestResultId)
                    );
            });

        }
    }
}
