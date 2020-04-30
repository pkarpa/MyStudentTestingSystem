using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    [Serializable]public class DTOAdministrator
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public ICollection<int> GroupsId { get; set; }

        public ICollection<int> SubjectsId { get; set; }

    }
}
