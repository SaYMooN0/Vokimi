﻿using VokimiShared.src.models.db_classes.users;
using VokimiShared.src.models.db_entities_ids;

namespace Vokimi.src.data.context_configuration.db_entities_relations_classes
{
    public class RelationsAppUserWithFollower
    {
        public AppUserId UserId { get; init; }
        public AppUserId FollowerId { get; init; }

        virtual public AppUser User { get; init; }
        virtual public AppUser Follower { get; init; }
    }
}
