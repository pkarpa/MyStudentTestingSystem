using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingSystemDatabaseLibrary;

namespace TestingSystemServer
{
    [Serializable]
    class TestingSystemDBContext: DbContext
    {
        //static TestingSystemDBContext()
        //{
        //    Database.SetInitializer<TestingSystemDBContext>(new ContextInitializer());
        //}
        public TestingSystemDBContext() : base("TestingSystemDBConnectionString")
        { 
            
        }

        public DbSet<Administrator> Administrators { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Theme> Themes { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<TestSession> TestSessions { get; set; }

        public DbSet<Answer> Answers { get; set; }
    }
}
