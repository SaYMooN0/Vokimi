using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vokimi.Models.DataBaseClasses
{
    public class Question
    {
        public uint Id { get; private set; }
        public string ImagePath { get; private set; }
        public string Text { get; private set; }
        private string answerOptionString { get; set; }
        [NotMapped]
        public Dictionary<string, int> AnswerOptions
        {
            get { return string.IsNullOrEmpty(answerOptionString) ? new() : JsonConvert.DeserializeObject<Dictionary<string, int>>(answerOptionString); }
            set { answerOptionString = JsonConvert.SerializeObject(value); }
        }
    }
}
