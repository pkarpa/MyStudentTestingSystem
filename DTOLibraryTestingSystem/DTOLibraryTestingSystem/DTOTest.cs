using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    
    [Serializable] public class DTOTest
    {      
        public int TestId { get; set; }

        public string TestName { get; set; }

        public DateTime TestTime { get; set; }

        public double TestCountScores { get; set; }

        public bool MixQuestionsOrder { get; set; }

        public bool MixAnswersOrder { get; set; }

        public int ThemeId { get; set; }

        public ICollection<int> GroupsId { get; set; }

        public ICollection<int> TestSessionsId { get; set; }
    }
}
