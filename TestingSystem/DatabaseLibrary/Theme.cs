using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace TestingSystemDatabaseLibrary
{  
   [Serializable]public class Theme
    {
        public int Id { get; set; }
        public string ThemeName { get; set; }
        public Subject Subject { get; set; }
    }
}
