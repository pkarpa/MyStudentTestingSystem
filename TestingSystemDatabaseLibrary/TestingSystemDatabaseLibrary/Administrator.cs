using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
   
     [Serializable]public class Administrator
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

        public Administrator()
        {
            Groups = new List<Group>();
            Subjects = new List<Subject>();
        }
    }
}
