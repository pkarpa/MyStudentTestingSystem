using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    [Serializable] public class DTOAnswer
    {
        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public bool AnswerCorrects { get; set; }

        public int QuestionId { get; set; }

        public int TestSessionId { get; set; }
    }
}
