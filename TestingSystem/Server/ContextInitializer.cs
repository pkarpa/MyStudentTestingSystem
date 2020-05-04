using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TestingSystemDatabaseLibrary;

namespace TestingSystemServer
{
    class ContextInitializer: DropCreateDatabaseIfModelChanges<TestingSystemDBContext>
    {
        protected override void Seed(TestingSystemDBContext db)
        {
            QuestionType q1 = new QuestionType { QuestionTypeName = "Single choice" };

            db.QuestionTypes.Add(q1);
            db.SaveChanges();
        }
    }
}
