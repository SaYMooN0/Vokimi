using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VokimiShared.src.models.db_classes.test_answers
{
    public class DraftTestQuestion
    {
        public DraftTestQuestionId Id { get; init; }
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public bool ShuffleAnswers { get; set; }
        public bool IsMultipleChoice { get; set; }
        public MultipleChoiceAdditionalData? MultipleChoiceData { get; set; }

        //public virtual ICollection<BaseAnswer> Answers { get; set; } = new List<BaseAnswer>();
        public int MinPossiblePoints() => throw new NotImplementedException();
        public int MaxPossiblePoints() => throw new NotImplementedException();
    }

    public class MultipleChoiceAdditionalData
    {
        public int MinAnswers { get; set; }
        public int MaxAnswers { get; set; }
        public bool UseAverageScore { get; set; }
    }

}
