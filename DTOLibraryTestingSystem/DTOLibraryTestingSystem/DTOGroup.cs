using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    
     [Serializable] public class DTOGroup
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int AdminId { get; set; }

        public ICollection<int> StudentsId { get; set; }

        public ICollection<int> TestsId { get; set; }
    }
}
