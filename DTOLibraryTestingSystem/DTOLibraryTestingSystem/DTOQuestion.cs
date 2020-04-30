using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    
    [Serializable] public class QueDTOQuestionstion
    {
        public int Id { get; set; }

        public string QuestionText { get; set; }

        public string AnswerOption { get; set; }

        public byte[] QuestionImage { get; set; }

        public double QuestionCountScores { get; set; }

        public string QuestionAnswer { get; set; }

        public int ThemeId { get; set; }

        public int QuestionTypeId { get; set; }
    }
}
