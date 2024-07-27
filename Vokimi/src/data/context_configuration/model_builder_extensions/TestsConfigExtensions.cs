using Microsoft.EntityFrameworkCore;
using Vokimi.src.data.context_configuration.db_entities_relations_classes;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test.test_questions;
using VokimiShared.src.models.db_classes.test.test_types;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    internal static class TestsConfigExtensions
    {
        internal static void ConfigureBaseTest(this ModelBuilder modelBuilder) {
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
                      .UsingEntity<TestWithTagRelations>(
                          j => j.HasOne(t => t.Tag).WithMany().HasForeignKey(t => t.TagId),
                          j => j.HasOne(t => t.Test).WithMany().HasForeignKey(t => t.TestId)
                      );
            });
        }

        internal static void ConfigureTestConclusion(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestConclusion>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestConclusionId(v));
            });
        }

        internal static void ConfigureTestStylesSheet(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestStylesSheet>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new TestStylesSheetId(v));
            });
        }

        internal static void ConfigureMultiChoiceQuestionData(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<MultiChoiceQuestionData>(entity =>
            {
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new MultiChoiceQuestionDataId(v));
            });
        }

        internal static void ConfigureTestTag(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<TestTag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new TestTagId(v));
                entity.Property(e => e.Value).IsRequired();
            });
        }
    }
}
