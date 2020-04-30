using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLibraryTestingSystem
{
    [Serializable] public class DTOQuestionType
    {
        public int Id { get; set; }
        public string QuestionTypeName { get; set; }
    }
}
