using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
   
     [Serializable] public class DTOStudent
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        public string SurName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public ICollection<int> TestSessionsId { get; set; }

    }
}
