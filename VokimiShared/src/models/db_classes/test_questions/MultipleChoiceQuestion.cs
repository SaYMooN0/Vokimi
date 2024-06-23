﻿using VokimiShared.src.models.db_classes.test_answers;

namespace VokimiShared.src.models.db_classes.test_questions
{
    public class MultipleChoiceQuestion<T> : BaseQuestion<T> where T : BaseAnswer
    {
        public int MinAnswers { get; init; }
        public int MaxAnswers { get; init; }
        public bool UseAverageScore { get; init; }
        public override int MaxPossiblePoints() {
            throw new NotImplementedException();
        }

        public override int MinPossiblePoints() {
            throw new NotImplementedException();
        }

    }
}
