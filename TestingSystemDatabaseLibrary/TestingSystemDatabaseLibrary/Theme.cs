using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestingSystemDatabaseLibrary
{  
   [Serializable]public class Theme
    {
        public int Id { get; set; }
        public string ThemeName { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public Theme()
        {
            Questions = new List<Question>();
        }
    }
}
