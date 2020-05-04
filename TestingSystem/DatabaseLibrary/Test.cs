using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
   [Serializable] public class Test
    {
        [Key]
        public int TestId { get; set; }

        public string TestName { get; set; }

        public DateTime TestTime { get; set; }

        public double TestCountScores { get; set; }

        public bool MixQuestionsOrder { get; set; }

        public bool MixAnswersOrder { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<TestSession> TestSessions { get; set; }

        public virtual Theme Theme { get; set; }

        public Test()
        {
            Groups = new List<Group>();
            TestSessions = new List<TestSession>();
        }
    }
}
