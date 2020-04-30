using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    [Serializable] public class DTOTheme
    {
        public int Id { get; set; }
        public string ThemeName { get; set; }
        public int SubjectId { get; set; }
        public ICollection<int> QuestionsId { get; set; }
    }
}
