using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TestViewModel
    {
        public int TestId { get; set; }

        public string TestName { get; set; }

        public DateTime TestTime { get; set; }

        public double TestCountScores { get; set; }

        public bool MixQuestionsOrder { get; set; }

        public bool MixAnswersOrder { get; set; }

        public int ThemeId { get; set; }

        public string ThemeName { get; set; }
    }
}
