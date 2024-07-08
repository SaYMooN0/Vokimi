using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VokimiShared.src.models.form_classes
{
    public class ConclustionCreationForm
    {
        public string Text { get; set; }
        public string? ImagePath { get; set; }
        public bool AddFeedback { get; set; }
        public string? FeedbackText { get; set; }
        public uint? MaxCharactersForFeedback { get; set; }
    }
}
