using System;
using System.Collections.Generic;
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
  
        public ICollection<Subject> Subjects { get; set; }

        public Administrator()
        {
            Subjects = new List<Subject>();
        }
    }
}
