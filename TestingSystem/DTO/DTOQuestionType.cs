using System;
using System.Collections.Generic;

namespace DTO
{
    [Serializable] public class DTOQuestionType
    {
        public int Id { get; set; }
        public string QuestionTypeName { get; set; }

        public ICollection<int> Questions { get; set; }
    }
}
