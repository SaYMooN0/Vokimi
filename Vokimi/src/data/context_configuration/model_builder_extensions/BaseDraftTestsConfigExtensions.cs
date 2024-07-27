﻿using Microsoft.EntityFrameworkCore;
using VokimiShared.src.models.db_classes.generic_test_answers;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.model_builder_extensions
{
    internal static class BaseDraftTestsConfigExtensions
    {
        internal static void ConfigureBaseDraftTest(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<BaseDraftTest>(entity =>
            {
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

       

        internal static void ConfigureDraftTestMainInfo(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestMainInfo>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestMainInfoId(v));
            });
        }

       

        internal static void ConfigureDraftTestResult(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestResult>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasConversion(v => v.Value, v => new DraftTestResultId(v));
                entity.Property(x => x.StringId).IsRequired();
                entity.Property(x => x.Text).IsRequired();
                entity.Property(x => x.ImagePath).IsRequired(false);

                entity.HasOne(x => x.TestTypeSpecificData)
                    .WithMany()
                    .HasForeignKey(x => x.TestTypeSpecificDataId)
                    .IsRequired();
            });
        }
        internal static void ConfigureDraftTestTypeSpecificResultData(this ModelBuilder modelBuilder) {
            modelBuilder.Entity<DraftTestTypeSpecificResultData>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasConversion(v => v.Value, v => new DraftTestTypeSpecificResultDataId(v));
            });

            modelBuilder.Entity<DraftGenericTestResultData>().ToTable("SpecificResultDataForGenericTest");
        }

    }
}
