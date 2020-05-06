using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
   
     [Serializable]public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Test> Tests { get; set; }

        public Group()
        {
            Students = new List<Student>();
            Tests = new List<Test>();
        }
    }
}
