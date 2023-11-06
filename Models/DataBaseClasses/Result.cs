using Newtonsoft.Json;

namespace Vokimi.Models.DataBaseClasses
{
    public class Result
    {
        public uint Id { get; private set; }
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
        public Result() { }
    }
}
