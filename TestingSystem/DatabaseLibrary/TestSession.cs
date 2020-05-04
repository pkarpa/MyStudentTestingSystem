using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
   
     [Serializable] public class TestSession
    {
        public int TestSessionId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Test Test { get; set; }
        public virtual Student Student { get; set; }

        public TestSession()
        {
            Answers = new List<Answer>();
        }
    }
}
