using Newtonsoft.Json;

namespace Vokimi.Models.DataBaseClasses
{
    public class Result
    {
        public int Id { get; private set; }
        public int TestId { get; private set; }
        public string Text { get; private set; }
        public string? Description { get; private set; }
        public string? ImagePath { get; private set; }
        public int GapMin { get; private set; }
        public int GapMax { get; private set; }
        [JsonConstructor]
        public Result(string text, string? description, int gapMin, int gapMax)
        {
            Text = text;
            Description = description;
            GapMin = gapMin;
            GapMax = gapMax;
        }
        public Result(int id, string text, string? imagePath, int gapMin, int gapMax, string? description, int testId)
        {
            Id = id;
            TestId = testId;
            Text = text;
            Description = description;
            ImagePath = imagePath;
            GapMin = gapMin;
            GapMax = gapMax;
        }
    }
}
