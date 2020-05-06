using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
    [Serializable]public class QuestionType
    {
        public int Id { get; set; }
        public string QuestionTypeName { get; set; }
        
        public ICollection<Question> Questions { get; set; }

        public QuestionType()
        {
            Questions = new List<Question>();
        }
    }
}
