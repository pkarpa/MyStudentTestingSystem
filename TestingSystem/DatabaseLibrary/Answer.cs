using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
   [Serializable] public class Answer
    {
        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public bool AnswerCorrects { get; set; }

        public Question Question { get; set; }

        public TestSession TestSession { get; set; }
    }
}
