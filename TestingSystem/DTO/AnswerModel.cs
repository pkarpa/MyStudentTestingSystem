using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AnswerModel
    {
        public int TestSessionId { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string GroupName { get; set; }

        public int QuestionCount { get; set; }

        public int CountCorrectAnswers { get; set; }

        public int CountInCorrectAnswers { get; set; }

        public double CountScores { get; set; }
    }
}
