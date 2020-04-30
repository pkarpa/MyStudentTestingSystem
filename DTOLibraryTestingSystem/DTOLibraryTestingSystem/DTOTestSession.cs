using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{   
     [Serializable] public class DTOTestSession
    {
        public int TestSessionId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<int> AnswersId { get; set; }
        public int TestId { get; set; }
        public int StudentId { get; set; }
    }
}
