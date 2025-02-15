﻿using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_results.results_for_draft_tests;
using VokimiShared.src.models.db_classes.tests;
using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.db_classes.test.test_types
{
    public abstract class BaseTest
    {
        public TestId Id { get; init; }
        public AppUserId CreatorId { get; init; }
        public virtual AppUser Creator { get; protected set; }
        public string Name { get; init; }
        public string Cover { get; init; }
        public string? Description { get; init; }
        public Language Language { get; init; }
        public TestPrivacy Privacy { get; init; }
        public DateTime CreationDate { get; init; }
        public DateTime PublicationDate { get; init; }

        public TestConclusionId? ConclusionId { get; init; }
        public virtual TestConclusion? Conclusion { get; protected set; }

        public TestStylesSheetId StylesSheetId { get; init; }
        public virtual TestStylesSheet StylesSheet { get; protected set; }

        public virtual ICollection<TestTag> Tags { get; protected set; } = [];

        public abstract TestTemplate Template { get; }

    }
}
