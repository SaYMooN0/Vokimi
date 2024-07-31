﻿using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.test_publishing_dtos
{
    public record BaseTestInfoPublishingDto(
        AppUserId CreatorId,
        string Name,
        string Cover,
        string? Description,
        Language Language,
        TestPrivacy Privacy,
        DateTime CreationDate,
        TestConclusionId? ConclusionId,
        TestStylesSheetId StylesSheetId)
    {
        public static BaseTestInfoPublishingDto FromDraftTest(BaseDraftTest test) => new(
            test.CreatorId,
            test.MainInfo.Name,
            test.MainInfo.CoverImagePath,
            test.MainInfo.Description,
            test.MainInfo.Language,
            test.MainInfo.Privacy,
            test.CreationDate,
            test.ConclusionId,
            test.StylesSheetId
            );
    }
}
