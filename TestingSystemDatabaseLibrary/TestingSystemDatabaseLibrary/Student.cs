﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystemDatabaseLibrary
{
    
    [Serializable]public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        public string SurName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<TestSession> TestSessions { get; set; }

        public Student()
        {
            TestSessions = new List<TestSession>();
        }
    }
}
