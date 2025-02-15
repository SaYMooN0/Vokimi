﻿using System.Runtime.InteropServices;
using VokimiShared.src.enums;
using VokimiShared.src.models.db_classes.test_creation;
using VokimiShared.src.models.db_entities_ids;

namespace VokimiShared.src.models.dtos
{
    public record TestPublishingDto(
        TestId Id,
        AppUserId CreatorId,
        string Name,
        string DraftTestCover,
        string? Description,
        Language Language,
        TestPrivacy Privacy,
        DateTime CreationDate,
        TestConclusionId? ConclusionId,
        TestStylesSheetId StylesSheetId)
    {
        public string NewCover { get; set; }
        public bool CoverRelocationNeeded => DraftTestCover != ImgOperationsHelper.DefaultTestCoverImg;

        public static TestPublishingDto FromBaseDraftTest(BaseDraftTest test) => new(
            new TestId(),
            test.CreatorId,
            test.MainInfo.Name,
            test.MainInfo.CoverImagePath,
            test.MainInfo.Description,
            test.MainInfo.Language,
            test.MainInfo.Privacy,
            test.CreationDate,
            test.ConclusionId,
            test.StylesSheetId
        ) {
            NewCover = ImgOperationsHelper.DefaultTestCoverImg
        };
    }

}
