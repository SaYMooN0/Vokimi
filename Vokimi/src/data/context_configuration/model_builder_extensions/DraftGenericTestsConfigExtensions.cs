using Microsoft.EntityFrameworkCore;
using Vokimi.src.data.context_configuration.db_entities_relations_classes;
using VokimiShared.src.models.db_classes.test_creation.generic_test_related;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    static internal class DraftGenericTestsConfigExtensions
    {
        internal static void ConfigureDraftGenericTest(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftGenericTest>(entity => {
                entity.HasMany(x => x.Questions)
                      .WithOne()
                      .HasForeignKey(x => x.DraftTestId)
                      .IsRequired();
            });
        }
        internal static void ConfigureDraftGenericTestQuestion(this ModelBuilder modelBuilder) {
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

        internal static void ConfigureDraftGenericTestAnswer(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftGenericTestAnswer>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new DraftTestAnswerId(v));
                entity.Property(e => e.QuestionId).IsRequired();
                entity.Property(e => e.OrderInQuestion).IsRequired();
                entity.HasOne(e => e.AdditionalInfo)
                      .WithOne()
                      .HasForeignKey<DraftGenericTestAnswer>(e => e.AdditionalInfoId);


                entity.HasMany(e => e.RelatedResultsData)
                    .WithMany(e => e.AnswersLeadingToResult)
                    .UsingEntity<RelationsDraftGenericTestAnswerResultData>(
                        j => j.HasOne(x => x.DraftTestResultData)
                              .WithMany()
                              .HasForeignKey(x => x.DraftTestResultDataId),
                        j => j.HasOne(x => x.DraftTestAnswer)
                              .WithMany()
                              .HasForeignKey(x => x.DraftTestAnswerId)
                    );
            });
        }
    }
}
