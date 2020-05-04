using System;
using System.Collections.Generic;

namespace DTO
{
    [Serializable] public class DTOSubject
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public ICollection<int> ThemesId { get; set; }

        public int AdminId { get; set; }
    }
}
