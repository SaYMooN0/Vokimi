using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VokimiShared.src.enums
{
    public enum AnswersType
    {
        TextOnly,
        TextAndImage,
        ImageOnly,
        //color
    }
    public static class AnswersTypeExtensions
    {
        public static bool HasImage(this AnswersType type) => type switch
        {
            AnswersType.TextOnly => false,
            AnswersType.TextAndImage => true,
            AnswersType.ImageOnly => true,
            _ => throw new NotImplementedException()
        };
    }
}
