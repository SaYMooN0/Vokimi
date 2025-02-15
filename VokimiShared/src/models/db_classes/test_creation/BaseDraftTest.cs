﻿using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test_creation
{
    public abstract class BaseDraftTest
    {
        public DraftTestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public DraftTestMainInfoId MainInfoId { get; init; }
        public virtual DraftTestMainInfo MainInfo { get; protected set; }
        public DateTime CreationDate { get; init; }
        public TestConclusionId? ConclusionId { get; protected set; }
        public virtual TestConclusion? Conclusion { get; protected set; }
        public TestTemplate Template { get; protected set; }
        public virtual ICollection<DraftTestResult> PossibleResults { get; set; } = [];
        public TestStylesSheetId StylesSheetId { get; set; }
        public virtual TestStylesSheet StylesSheet { get; set; }

        public void AddConclusion(TestConclusion conclusion) {
            ConclusionId = conclusion.Id;
            Conclusion = conclusion;
        }
        public void RemoveConclusion() {
            ConclusionId = null;
            Conclusion = null;
        }
    }
}
