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

        public ICollection<Answer> Answers { get; set; }
        public Test Test { get; set; }
        public Student Student { get; set; }

        public TestSession()
        {
            Answers = new List<Answer>();
        }
    }
}
