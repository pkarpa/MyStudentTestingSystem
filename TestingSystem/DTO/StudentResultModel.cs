using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class StudentResultModel
    {
        public string TestName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int QuestionCount { get; set; }

        public int CountCorrectAnswers { get; set; }

        public int CountInCorrectAnswers { get; set; }

        public double CountScores { get; set; }
    }
}
