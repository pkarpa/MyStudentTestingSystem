using System;
using System.Collections.Generic;

namespace DTO
{
    [Serializable] public class DTOTheme
    {
        public int Id { get; set; }
        public string ThemeName { get; set; }
        public int SubjectId { get; set; }
        public ICollection<int> QuestionsId { get; set; }
    }
}
