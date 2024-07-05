using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VokimiShared.src.models.db_classes.test_answers
{
    public class ImageOnlyAnswer : BaseAnswer
    {
        public string ImagePath { get; init; }
        public static ImageOnlyAnswer CreateNew(
            DraftTestQuestionId questionId,
            ushort order,
            string imagePath) =>
                new() {
                    AnswerId = new(),
                    QuestionId = questionId,
                    OrderInQuestion = order,
                    ImagePath = imagePath
                };

    }
}
