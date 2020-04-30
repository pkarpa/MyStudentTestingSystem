using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    [Serializable] public class DTOSubject
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public ICollection<int> ThemesId { get; set; }

        public int AdminId { get; set; }
    }
}
