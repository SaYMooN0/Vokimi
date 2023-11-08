using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vokimi.Models.DataBaseClasses
{
    public class Question
    {
        public int Id { get; private set; }
        public int TestId { get; private set; }
        public string ImagePath { get; private set; }
        public string Text { get; private set; }
        public string answerOptionString { get; private set; }
        [NotMapped]
        public Dictionary<string, int> AnswerOptions
        {
            get { return string.IsNullOrEmpty(answerOptionString) ? new() : JsonConvert.DeserializeObject<Dictionary<string, int>>(answerOptionString); }
            set { answerOptionString = JsonConvert.SerializeObject(value); }
        }
        [JsonConstructor]
        public Question(string text, Dictionary<string, int> answerOptions)
        {
            Text = text;
            AnswerOptions = answerOptions;
        }

        public Question(){}
    }
}
