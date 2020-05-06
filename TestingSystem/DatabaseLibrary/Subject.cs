using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
    [Serializable]public class Subject
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public ICollection<Theme> Themes { get; set; }

        public Administrator Admin { get; set; }

        public Subject()
        {
            Themes = new List<Theme>();
        }
    }
}
