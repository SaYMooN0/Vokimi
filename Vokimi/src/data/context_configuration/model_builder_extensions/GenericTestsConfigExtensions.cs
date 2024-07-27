using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_entities_ids;
using VokimiShared.src.models.db_classes.generic_test_answers;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    internal static class GenericTestsConfigExtensions
    {
        internal static void ConfigureTestGenericType(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestGenericType>(entity => {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.TestId);
            });
        }

        internal static void ConfigureGenericTestQuestion(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<GenericTestQuestion>(entity => {
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new QuestionId(v));
                entity.Property(x => x.MultiChoiceQuestionDataId).HasConversion(
                    v => v.HasValue ? v.Value.Value : (Guid?)null,
                    v => v.HasValue ? new MultiChoiceQuestionDataId(v.Value) : null);
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
    }
}
