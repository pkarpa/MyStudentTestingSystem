using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Data.Entity;
using TestingSystemDatabaseLibrary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DTO;

namespace TestingSystemServer
{
  // клас клієнта
  // опрацьовує запити клієнти
  public class ClientObject
  {
    protected internal string ClientObjectId { get; private set; } // id клієнта
    protected internal NetworkStream Stream { get; private set; } // потік через який відбувається обмін повідомленнями

    string name;
    string login;
    string password;
    bool isAdmin;
    TcpClient client;
    ServerObject server;

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
      ClientObjectId = Guid.NewGuid().ToString(); // формуємо id для клієнта  
      client = tcpClient; // об'єкт клієнта
      server = serverObject; // об'єкт сервера
      serverObject.AddConnection(this); // додаємо нове з'єднання
      name = "";
      login = "";
      password = "";
      isAdmin = false;
    }


    // Метод, який приймає запити з клієнтської частини
    public void Process()
    {
      try
      {
        Stream = client.GetStream(); // потік для надсилання та приймання повідомлень

        while (true) // цикл для прийому вхідних повідомлень
        {
          string[] message = GetMessage().Split(':'); // отримуємо вхідне повідомлення та виділяємо назву методу яка міститься до :

          switch (message[0]) // викликається певний метод відповідно до назви, яку ми отримади з вхідного повідомлення
          {
            case "AdminRegistration":
              AdminRegistration();
              Console.WriteLine(this.ClientObjectId + ": AdminRegistration");
              break;
            case "StudentRegistration":
              StudentRegistration();
              Console.WriteLine(this.ClientObjectId + ": StudentRegistration");
              break;
            case "LogIn":
              LogIn(message[1]);
              Console.WriteLine(this.ClientObjectId + ": LogIn");
              break;
            case "InfoAboutAdmin":
              InfoAboutAdmin(message[1]);
              Console.WriteLine(this.ClientObjectId + ": InfoAboutAdmin");
              break;
            case "EditAdminName":
              EditAdminName(message[1]);
              Console.WriteLine(this.ClientObjectId + ": EditAdminName");
              break;
            case "EditAdminLogin":
              EditAdminLogin(message[1]);
              Console.WriteLine(this.ClientObjectId + ": EditAdminLogin");
              break;
            case "EditAdminPassword":
              EditAdminPassword(message[1]);
              Console.WriteLine(this.ClientObjectId + ": EditAdminPassword");
              break;
            case "GetGroups":
              GetGroups();
              Console.WriteLine(this.ClientObjectId + ": GetGroups");
              break;
            case "GetTheme":
              GetTheme();
              Console.WriteLine(this.ClientObjectId + ": GetTheme");
              break;
            case "GetQuestions":
              GetQuestions(message[1]);
              Console.WriteLine(this.ClientObjectId + ": GetQuestions");
              break;
            case "SaveQuestion":
              SaveQuestion(message[1]);
              Console.WriteLine(this.ClientObjectId + ": SaveQuestion");
              break;
            case "UpdateQuestion":
              UpdateQuestion(message[1]);
              Console.WriteLine(this.ClientObjectId + ": SaveQuestion");
              break;
            case "GetQuestionTypes":
              GetQuestionTypes();
              Console.WriteLine(this.ClientObjectId + ": GetQuestionTypes");
              break;
            case "AddGroup":
              AddGroup(message[1]);
              Console.WriteLine(this.ClientObjectId + ": AddGroup");
              break;
            case "DeleteGroup":
              DeleteGroup(message[1]);
              Console.WriteLine(this.ClientObjectId + ": DeleteGroup");
              break;
            case "GetGroupStudents":
              GetGroupStudents();
              Console.WriteLine(this.ClientObjectId + ": GetGroupStudents");
              break;
            case "GetGroupTests":
              GetGroupTests();
              Console.WriteLine(this.ClientObjectId + ": GetGroupStudents");
              break;
            case "GetAllTests":
              GetAllTests();
              Console.WriteLine(this.ClientObjectId + ": GetGroupStudents");
              break;
            case "DeleteStudentFromGroup":
              DeleteStudentFromGroup(message[1]);
              Console.WriteLine(this.ClientObjectId + ": DeleteStudentFromGroup");
              break;
            case "InfoAboutStudent":
              InfoAboutStudent(message[1]);
              Console.WriteLine(this.ClientObjectId + ": InfoAboutStudent");
              break;
            case "GetTestsForSomeGroup":
                GetTestsForSomeGroup(message[1]);
                Console.WriteLine(this.ClientObjectId + ": GetTestsForSomeGroup");
                break;
            case "GetTest":
                GetTest(message[1]);
                Console.WriteLine(this.ClientObjectId + ": GetTest");
                break;              
            case "GetThemesForSubject":
                GetThemesForSubject(message[1]);
                Console.WriteLine(this.ClientObjectId + ": GetThemesForSubject");
                break; 
            case "GetTestsForTheme":
                GetTestsForTheme(message[1]);
                Console.WriteLine(this.ClientObjectId + ": GetTestsForTheme");
                break; 
            case "GetSubjectsForAdmin":
                GetSubjectsForAdmin(message[1]);
                Console.WriteLine(this.ClientObjectId + ": GetSubjectsForAdmin");
                break;
            case "AddTestSession":
                AddTestSession();
                Console.WriteLine(this.ClientObjectId + ": AddTestSession");
                break;
            case "AddStudentsAnswers":
                AddStudentsAnswers();
                Console.WriteLine(this.ClientObjectId + ": AddStudentsAnswers");
                break;
            case "UpdateTestSession":
                UpdateTestSession();
                Console.WriteLine(this.ClientObjectId + ": UpdateTestSession");
                break;
            case "LogOut":
              Console.WriteLine(this.ClientObjectId + ": LogOut");
              LogOut();
              return;
          }
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
      finally
      {
        //server.RemoveConnection(this.ClientObjectId);
        //Close();
      }
    }

    // Метод для входу користувача в систему
    // Приймає стрічку, яка містить ідентифікатор, який визначає який користувач входить (студент/адмін), також стрічка містить логін та пароль користувача
    // Метод перевіряє чи існує користувач з даним логіном та паролем в базі, і надсилає відповідь клієнтській частині
    //private void LogIn(string mess)
    //{
    //    string[] logEl = mess.Split();
    //    isAdmin = bool.Parse(logEl[0]);
    //    login = logEl[1];
    //    password = logEl[2];

    //    if (isAdmin)
    //    {
    //        using (var db = new TestingSystemDBContext())
    //        {
    //            if (db.Administrators.FirstOrDefault(a => a.Login == login) != null)
    //            {
    //                if (db.Administrators.FirstOrDefault(a => a.Password == password) != null)
    //                {
    //                    var admin = db.Administrators.FirstOrDefault(a => a.Password == password);
    //                    SendMessage(admin.Id + ":succesfully");
    //                }
    //                else
    //                {
    //                    SendMessage("No user with this password");
    //                }
    //            }
    //            else
    //            {
    //                SendMessage("No user with this login");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        using (var db = new TestingSystemDBContext())
    //        {
    //            if (db.Students.FirstOrDefault(s => s.Login == login) != null)
    //            {
    //                if (db.Students.FirstOrDefault(a => a.Password == password) != null)
    //                {
    //                    var student = db.Students.FirstOrDefault(s => s.Password == password);
    //                    SendMessage(student.StudentId + ":succesfully");
    //                }
    //                else
    //                {
    //                    SendMessage("No user with this password");
    //                }
    //            }
    //            else
    //            {
    //                SendMessage("No user with this login");
    //            }
    //        }
    //    }
    //}
    private void LogIn(string mess)
    {
      string[] logEl = mess.Split();
      isAdmin = false;
      login = logEl[0];
      password = logEl[1];
      bool loginResult = false;

      using (var db = new TestingSystemDBContext())
      {
        //Group group = db.Groups.FirstOrDefault(x => x.GroupName == "PM1");
        //Test test = db.Tests.FirstOrDefault(t => t.TestId == 1);
        //test.Groups.Add(group);
        //db.SaveChanges();
        var admin = db.Administrators.FirstOrDefault(a => a.Login == login && a.Password == password);
        if (admin != null)
        {
          isAdmin = true;
          SendMessage(admin.Id + " " + isAdmin + ":succesfully");
          loginResult = true;
        }
        else
        {
          var student = db.Students.FirstOrDefault(s => s.Login == login && s.Password == password);
          if (student != null)
          {
            SendMessage(student.StudentId + " " + isAdmin + ":succesfully");
            loginResult = true;
          }
        }

        if (!loginResult)
        {
          SendMessage("Login failed, please check username or password.");
        }
      }
    }

    private void LogOut()
    {
      server.RemoveConnection(ClientObjectId);
      this.Close();
    }

    public void AddTestSession()
    {
        var obj = RecieveObject();
        int testSessionId = -1;
        if (obj is DTOTestSession)
        {
            DTOTestSession dtoTestSession = obj as DTOTestSession;
            using (var db = new TestingSystemDBContext())
            {
                Student student = db.Students.Include(x => x.Group).Include(d=>d.TestSessions).FirstOrDefault(s => s.StudentId == dtoTestSession.StudentId);
                Test test = db.Tests.Include(x => x.Groups).Include(c=>c.Questions).Include(p=>p.Theme).FirstOrDefault(t => t.TestId == dtoTestSession.TestId);
                TestSession testSession = new TestSession() { StartTime = dtoTestSession.StartTime, EndTime = DateTime.Today, Status = dtoTestSession.Status, Student = student, Test = test, Answers = null};
                db.TestSessions.Add(testSession);
                testSessionId = testSession.TestSessionId;
                string answer = testSessionId.ToString();
                SendMessage(answer);
                db.SaveChanges();              
            }
        }
        else 
        {
            Console.WriteLine("Something wrong with adding new test session!");
        }
    }

    public void AddStudentsAnswers()
    {
        var obj = RecieveObject();
        if (obj is List<DTOAnswer>)
        {
            List<DTOAnswer> dtoAnswers = obj as List<DTOAnswer>;
            List<Answer> answers = new List<Answer>();
            string message = null;
            using (var db = new TestingSystemDBContext())
            {
                foreach (var item in dtoAnswers)
                {
                    Question question = db.Questions.FirstOrDefault(q => q.Id == item.QuestionId);
                    TestSession testSession = db.TestSessions.FirstOrDefault(s => s.TestSessionId == item.TestSessionId);
                    Answer answer = new Answer() { AnswerText = item.AnswerText, AnswerCorrects = item.AnswerCorrects, Question = question, TestSession = testSession };
                    answers.Add(answer);
                }
                db.Answers.AddRange(answers);
                foreach (var i in answers)
                {
                    message += i.AnswerId + " ";
                }
                db.SaveChanges();
                SendMessage(message);
            }
        }
        else
        {
            Console.WriteLine("Something wrong with adding answers on test session!");
        }
    }

    public void UpdateTestSession()
    {
        var obj = RecieveObject();
        if (obj is DTOTestSession)
        {
            DTOTestSession dtoTestSession = obj as DTOTestSession;
            List<Answer> answers = new List<Answer>();
            using (var db = new TestingSystemDBContext())
            {
                    foreach (var i in dtoTestSession.AnswersId)
                    {
                        Answer answer = db.Answers.FirstOrDefault(a => a.AnswerId == i);
                        answers.Add(answer);
                    }
                TestSession testSession = db.TestSessions.FirstOrDefault(t => t.TestSessionId == dtoTestSession.TestSessionId);
                testSession.Status = dtoTestSession.Status;
                testSession.EndTime = dtoTestSession.EndTime;
                testSession.Answers = answers;
                db.SaveChanges();
            }
        }
        else
        {
            Console.WriteLine("Something wrong with update test session!");
        }
    }

    private void GetTest(string mess)
    {
        int testId = int.Parse(mess);
        DTOTest dtoTest = null;
        using (var db = new TestingSystemDBContext())
        {
            Test test = db.Tests.Include(x => x.Theme).FirstOrDefault(t => t.TestId == testId);
            dtoTest = new DTOTest { TestId = test.TestId, TestName = test.TestName, TestTime = test.TestTime, MixAnswersOrder = test.MixAnswersOrder, MixQuestionsOrder = test.MixQuestionsOrder, TestCountScores = test.TestCountScores, ThemeId = test.Theme.Id };
        }

        SendObject(dtoTest);
    }
        
    private void GetSubjectsForAdmin(string mess)
    {
        List<DTOSubject> subjects = new List<DTOSubject>();
        using (var db = new TestingSystemDBContext())
        {
            int adminId = int.Parse(mess);
            var subjectDB = db.Subjects.Include(a => a.Admin).ToList();
            foreach (var subjectItem in subjectDB)
            {
                if (subjectItem.Admin.Id == adminId)
                {
                    DTOSubject dtoSubject = new DTOSubject
                    {
                        SubjectId = subjectItem.SubjectId,
                        SubjectName = subjectItem.SubjectName
                    };
                    subjects.Add(dtoSubject);
                }    
            }
            SendObject(subjects);
        }
    }

    private void GetThemesForSubject(string mess)
    {
        List<DTOTheme> themes = new List<DTOTheme>();
        using (var db = new TestingSystemDBContext())
        {
            int subjectId = int.Parse(mess);
            var themeDB = db.Themes.Include(x => x.Subject).ToList();
            foreach (var themeItem in themeDB)
            {
                if (themeItem.Subject.SubjectId == subjectId)
                {
                    DTOTheme dtoTheme = new DTOTheme
                    {
                        Id = themeItem.Id,
                        SubjectId = subjectId,
                        ThemeName = themeItem.ThemeName
                    };
                    themes.Add(dtoTheme);
                }
            }
            SendObject(themes);
        }
    }
        

    private void GetTestsForTheme(string mess)
    {
        List<DTOTest> tests = new List<DTOTest>();
        using (var db = new TestingSystemDBContext())
        {
            int testId = int.Parse(mess);
            var testDB = db.Tests.Include(x => x.Theme).ToList();
            var testSessionDB = db.TestSessions.Include(x => x.Test).ToList();
            foreach (var testItem in testDB)
            {
                List<int> testSessionsId = new List<int>();
                foreach (var testSession in testSessionDB)
                {
                    if (testSession.Test.TestId == testItem.TestId)
                    {
                        testSessionsId.Add(testSession.TestSessionId);
                    }
                }

                if (testItem.Theme.Id == testId)
                {
                    DTOTest dtoTest = new DTOTest
                    {
                        TestId = testItem.TestId,
                        TestName = testItem.TestName,
                        MixAnswersOrder = testItem.MixAnswersOrder,
                        MixQuestionsOrder = testItem.MixQuestionsOrder,
                        TestCountScores = testItem.TestCountScores,
                        TestTime = testItem.TestTime,
                        ThemeId = testItem.Theme.Id,
                        TestSessionsId = testSessionsId
                    };
                    tests.Add(dtoTest);
                }
            }
            SendObject(tests);
        }
    }
        // Реєстрація нового адміністратора
        private void AdminRegistration()
    {
      var obj = RecieveObject(); // отримуємо об'єкт адміністратора з клієнтської частини 

      if (obj is DTOAdministrator) // перевіряємо чи це справді об'єкт адміністратора
      {
        DTOAdministrator dtoAdmin = obj as DTOAdministrator; // приведення до потрібного типу
        using (var db = new TestingSystemDBContext())
        {
          // якщо такого адміністратора ще не існує в базі то додаємо дані нового адміна в базу
          if (db.Administrators.FirstOrDefault(a => a.Login == dtoAdmin.Login) == null)
          {
            Administrator administrator = new Administrator() { Name = dtoAdmin.Name, Login = dtoAdmin.Login, Password = dtoAdmin.Password };
            db.Administrators.Add(administrator);
            db.SaveChanges();
            SendMessage("successfully");
          }
          else
          {
            SendMessage("login already exists");
          }
        }
      }
      else
      {
        SendMessage("Error object!");
      }
    }

    //Реєстрація нового студента
    private void StudentRegistration()
    {
      var obj = RecieveObject(); // Отримуємо об'єкт студента з клієнтської частини

      if (obj is DTOStudent) // перевіряємо чи це справді об'єкт студента
      {
        DTOStudent dtoStudent = obj as DTOStudent; // приведення до потрібного типу
        using (var db = new TestingSystemDBContext())
        {
          // якщо такого студента ще не існує в базі то додаємо дані нового адміна в базу
          if (db.Students.FirstOrDefault(s => s.Login == dtoStudent.Login) == null)
          {
            Group gr = db.Groups.FirstOrDefault(g => g.GroupId == dtoStudent.GroupId);

            Student student = new Student() { Name = dtoStudent.Name, SurName = dtoStudent.SurName, Group = gr, Login = dtoStudent.Login, Password = dtoStudent.Password };

            db.Students.Add(student);
            db.SaveChanges();
            SendMessage("successfully");
          }
          else
          {
            SendMessage("login already exists");
          }
        }
      }
      else
      {
        SendMessage("Error object!");
      }
    }



    //Видалення певної групи студентів
    public void DeleteGroup(string mess)
    {
      int groupId = Convert.ToInt32(mess); // Отримуємо з повідомлення id потрібної групи
      using (var db = new TestingSystemDBContext())
      {
        Group gr = db.Groups.FirstOrDefault(g => g.GroupId == groupId);
        db.Groups.Remove(gr); // видаляємо групу
        db.SaveChanges();
        SendMessage("successfully");
      }
    }

    // Видалення студента з певної групи студентів
    private void DeleteStudentFromGroup(string _id)
    {
      int id = int.Parse(_id); // Отримуємо id студента
      using (var db = new TestingSystemDBContext())
      {
        Student student = db.Students.Include(g => g.Group).FirstOrDefault(s => s.StudentId == id); // Отримуємо студента з бази по його id
        Group group = db.Groups.Include(s => s.Students).FirstOrDefault(g => g.GroupId == student.Group.GroupId); // Отримуємо групу жо якої належить цей студент
        group.Students.Remove(student); // Видаляємо студента з групи
        db.SaveChanges();
      }
      SendMessage("successfully");
    }

    // Метод який повертає студентів певної групи
    private void GetGroupStudents()
    {
      var obj = RecieveObject(); // отримуємо список  id студентів певної групи
      if (obj is int)
      {
        List<DTOStudent> students = new List<DTOStudent>();// Список студентів
        using (var db = new TestingSystemDBContext())
        {
          var studentsDB = db.Students.Include(g => g.Group).Where(s => s.Group.GroupId == (int)obj);
          foreach (var student in studentsDB)
          {
            DTOStudent dtoStudent = new DTOStudent { StudentId = student.StudentId, Name = student.Name, SurName = student.SurName, Login = student.Login, Password = student.Password, GroupId = student.Group.GroupId, GroupName = student.Group.GroupName }; // Створюємо об'єкт студента для надсилання
            students.Add(dtoStudent); // Додаємо студента до списку 
          }

          SendObject(students); // Надсилаємо список студентів на клієнтську частину
        }
      }
    }

    private void GetTheme()
    {
        List<DTOTheme> theme = new List<DTOTheme>();
        using (var db = new TestingSystemDBContext())
        {
          var themeDB = db.Themes.Include(g => g.Subject).ToList();
          foreach (var themeItem in themeDB)
          {
            DTOTheme dtoTheme = new DTOTheme
            {
              SubjectId = themeItem.Subject.SubjectId,
              ThemeName = themeItem.ThemeName
            };
            theme.Add(dtoTheme);
          }

          SendObject(theme);
        }
    }


    private void GetQuestions(string testId)
    {
      List<QueDTOQuestionstion> questions = new List<QueDTOQuestionstion>();
      var obj = RecieveObject();
      int testIdNum = Convert.ToInt32(obj);
      using (var db = new TestingSystemDBContext())
      {
        var testDB = db.Tests.Include(g => g.Questions).Where(x => x.TestId == testIdNum).ToList();
        var questionsDb = testDB.FirstOrDefault().Questions;
        foreach (var questionItem in questionsDb)
        {
          QueDTOQuestionstion dtoQuestion = new QueDTOQuestionstion
          {
            Id = questionItem.Id,
            AnswerOption = questionItem.AnswerOption,
            QuestionAnswer = questionItem.QuestionAnswer,
            QuestionCountScores = questionItem.QuestionCountScores,
            QuestionText = questionItem.QuestionText,
            QuestionImage = questionItem.QuestionImage,
            QuestionTypeId = questionItem.QuestionTypeId,
          };
          questions.Add(dtoQuestion);
        }

        SendObject(questions);
      }
    }

    private void SaveQuestion(string testId)
    {
      var obj = RecieveObject();
      var result = 0;
      try
      {
        if (obj is QueDTOQuestionstion) // перевіряємо чи це справді об'єкт студента
        {
          QueDTOQuestionstion question = obj as QueDTOQuestionstion;
          if (question.QuestionTypeId == 0)
            question.QuestionTypeId = 1;
          using (var db = new TestingSystemDBContext())
          {
            var questionDB = new Question()
            {
              AnswerOption = question.AnswerOption,
              //Id = question.Id,
              QuestionAnswer = question.QuestionAnswer,
              QuestionCountScores = question.QuestionCountScores,
              QuestionImage = question.QuestionImage,
              QuestionText = question.QuestionText,
              QuestionTypeId = question.QuestionTypeId,
            };
            db.Questions.Add(questionDB);
            db.SaveChanges();
            db.Entry(questionDB).GetDatabaseValues();
            var t = db.Entry(questionDB);
            int id = questionDB.Id;
            result = id;
          }
        }
      }
      catch (Exception e)
      {
      }
        SendObject(result);
    }

    private void UpdateQuestion(string testId)
    {
      var obj = RecieveObject();
      var result = false;
      try
      {
        if (obj is QueDTOQuestionstion) // перевіряємо чи це справді об'єкт студента
        {
          QueDTOQuestionstion question = obj as QueDTOQuestionstion;
          using (var db = new TestingSystemDBContext())
          {
            Question record = db.Questions.Where(x => x.Id == question.Id).FirstOrDefault();
            record.AnswerOption = question.AnswerOption;
            record.QuestionAnswer = question.QuestionAnswer;
            record.QuestionCountScores = question.QuestionCountScores;
            record.QuestionImage = question.QuestionImage;
            record.QuestionText = question.QuestionText;
            record.QuestionTypeId = question.QuestionTypeId;
            db.SaveChanges();
            result = true;
          }
        }
      }
      catch (Exception e)
      {

      }
      SendObject(result);
    }

    private void GetQuestionTypes()
    {
      List<DTOQuestionType> questionTypes = new List<DTOQuestionType>();
      using (var db = new TestingSystemDBContext())
      {
        var questionTypeDb = db.QuestionTypes.ToList();
        foreach (var questionTypeItem in questionTypeDb)
        {
          DTOQuestionType dtoQuestionType = new DTOQuestionType
          {
            Id = questionTypeItem.Id,
            QuestionTypeName = questionTypeItem.QuestionTypeName,
          };
          questionTypes.Add(dtoQuestionType);
        }

        SendObject(questionTypes);
      }
    }



    // Метод який повертає тести які доступні для певної групи студентів
    private void GetGroupTests()
    {
      var obj = RecieveObject(); // отримуємо список id тестів, які доступні для певної групи студентів
      if (obj is List<int>)
      {
        List<DTOTest> tests = new List<DTOTest>(); // Список тестів
        List<int> testsId = (List<int>)obj; // Приведення до потрібного типу
        using (var db = new TestingSystemDBContext())
        {
          for (int i = 0; i < testsId.Count(); i++)
          {
            int id = testsId[i];
            Test test = db.Tests.Include(g => g.Theme).FirstOrDefault(s => s.TestId == id); // Отримуємо тест з бази по його id
            DTOTest dtoTest = new DTOTest { TestId = test.TestId, TestName = test.TestName, TestTime = test.TestTime, MixAnswersOrder = test.MixAnswersOrder, MixQuestionsOrder = test.MixQuestionsOrder, TestCountScores = test.TestCountScores, ThemeId = test.Theme.Id }; // Створюємо об'єкт теста для надсилання
            tests.Add(dtoTest); // Додаємо тест до списку 
          }
        }

        SendObject(tests); // Надсилаємо список тестів на клієнтську частину          
      }
    }

    private void GetAllTests()
    {
      List<DTOTest> testsDto = new List<DTOTest>(); // Список тестів
      using (var db = new TestingSystemDBContext())
      {
        List<Test> tests = db.Tests.Include(g => g.Theme).ToList();
        foreach (var test in tests)
        {
          DTOTest dtoTest = new DTOTest { TestId = test.TestId, TestName = test.TestName, TestTime = test.TestTime, MixAnswersOrder = test.MixAnswersOrder, MixQuestionsOrder = test.MixQuestionsOrder, TestCountScores = test.TestCountScores, ThemeId = test.Theme.Id }; // Створюємо об'єкт теста для надсилання
          testsDto.Add(dtoTest); // Додаємо тест до списку 
        }
      }

      SendObject(testsDto); // Надсилаємо список тестів на клієнтську частину          
    }

    public void GetTestsForSomeGroup(string mess)
    {
        int studentId = int.Parse(mess);
        List<DTOTest> testsDto = new List<DTOTest>(); // Список тестів
        using (var db = new TestingSystemDBContext())
        {
            Group group = db.Students.Include(g => g.Group).FirstOrDefault(s => s.StudentId == studentId).Group;
            List<Test> tests = db.Tests.Include(g => g.Groups).Include(t => t.Theme).ToList();
            foreach (var test in tests)
            {
                if (test.Groups.Contains(group))
                {
                    DTOTest dtoTest = new DTOTest { TestId = test.TestId, TestName = test.TestName, TestTime = test.TestTime, MixAnswersOrder = test.MixAnswersOrder, MixQuestionsOrder = test.MixQuestionsOrder, TestCountScores = test.TestCountScores, ThemeId = test.Theme.Id }; // Створюємо об'єкт теста для надсилання
                    testsDto.Add(dtoTest); // Додаємо тест до списку 
                }
            }
        }
        SendObject(testsDto); // Надсилаємо список тестів на клієнтську частину        
    }


        // Метод який повертає інформацію про певного адміністратора
        private void InfoAboutAdmin(string mess)
    {
      int adminId = int.Parse(mess); // отримуємо id адміністратора з повідомлення
      using (var db = new TestingSystemDBContext())
      {
        var admin = db.Administrators.FirstOrDefault(a => a.Id == adminId); // Отримуємо адміністратора з бази по id
        List<int> subjectsId = new List<int>(); // список предметів, які створив цей адміністратор
        if (admin.Subjects.Count != 0)
        {
          foreach (var item in admin.Subjects)
          {
            subjectsId.Add(item.SubjectId);
          }
        }
        DTOAdministrator administrator = new DTOAdministrator { Id = admin.Id, Name = admin.Name, Login = admin.Login, Password = admin.Password, SubjectsId = subjectsId }; // створення об'єкта адміністратора для надсилання 
        SendObject(administrator); // надсилання об'єкта адміністратора на клієнтську частину
      }
    }

    // Метод який повертає інформацію Subjects
    private void GetSubjects(string mess)
    {
      int adminId = int.Parse(mess); // отримуємо id адміністратора з повідомлення
      using (var db = new TestingSystemDBContext())
      {
        var subjects = db.Subjects.Where(x => x.Admin.Id == adminId).ToList(); // Отримуємо адміністратора з бази по id
        List<DTOSubject> subjectsId = new List<DTOSubject>(); // список предметів, які створив цей адміністратор
        foreach (var subject in subjects)
        {
          DTOSubject item = new DTOSubject
          {
            SubjectId = subject.SubjectId,
            SubjectName = subject.SubjectName,
            ThemesId = subject.Themes.Select(x => x.Id).ToList(),
            AdminId = subject.Admin.Id
          };
          subjectsId.Add(item);
        }

        SendObject(subjectsId); // надсилання об'єкта адміністратора на клієнтську частину
      }
    }

    // Метод який повертає інформацію про певного студента
    private void InfoAboutStudent(string mess)
    {
      int studentId = int.Parse(mess); // отримуємо id студента з повідомлення
      using (var db = new TestingSystemDBContext())
      {
        var student = db.Students.FirstOrDefault(s => s.StudentId == studentId); // Отримуємо студента з бази по id
        List<int> testSessionsId = new List<int>(); // список тестових сесій, які проходив цей студент
        if (student.TestSessions.Count != 0)
        {
          foreach (var item in student.TestSessions)
          {
            testSessionsId.Add(item.TestSessionId);
          }
        }
        DTOStudent dtoStudent = new DTOStudent { StudentId = student.StudentId, Name = student.Name, SurName = student.SurName, Login = student.Login, Password = student.Password, GroupId = 1, GroupName = "PM1", TestSessionsId = testSessionsId }; // створення об'єкта студента для надсилання 
        SendObject(dtoStudent); // надсилання об'єкта студента на клієнтську частину
      }
    }

    // Створення та збереження нової групи студентів в базу 
    private void AddGroup(string mess)
    {
      string groupName = mess; // назва групи
      using (var db = new TestingSystemDBContext())
      {
        // перевірка чи не існує вже групи з таким іменем в базі 
        if (db.Groups.FirstOrDefault(g => g.GroupName == groupName) == null)
        {
          Group gr = new Group() { GroupName = groupName }; // Створення об'єкта групи
          db.Groups.Add(gr); // додаємо групу до бази
          db.SaveChanges();
          SendMessage("successfully"); // відправляємо повідомлення про успішність операції
        }
        else
        {
          SendMessage("ExistName");
        }
      }
    }


    // Метод який приймає повідомлення з клієнтської частини
    private string GetMessage()
    {
      byte[] data = new byte[64]; // масив для зберігання даних з потоку  
      StringBuilder builder = new StringBuilder(); // об'єкт для зберігання повідомлення 
      int bytes = 0;
      // Зчитуємо дані з потоку, поки вони там є  
      do
      {
        bytes = Stream.Read(data, 0, data.Length); // розмір даних які були додані в масив data
        builder.Append(Encoding.UTF8.GetString(data, 0, bytes)); // витягуємо дані з масиву байт data, перетворююси їх в String 
      }
      while (Stream.DataAvailable);

      return builder.ToString(); // Повертаємо отримане повідомлення
    }

    // Метод який надсилає повідомлення типу String на клієнтську частину
    private void SendMessage(string mess)
    {
      byte[] data = Encoding.UTF8.GetBytes(mess); // перетворюємо повідомлення типу String в масив байт
      Stream.Write(data, 0, data.Length); // Відправдяємо дані 
    }

    // Метод який надсилає об'єкти на клієнтську частину
    private void SendObject(Object obj)
    {
      IFormatter formatter = new BinaryFormatter();
      formatter.Serialize(Stream, obj);
    }

    // Метод який приймає об'єкти з клієнтської частини
    public Object RecieveObject()
    {
      IFormatter formatter = new BinaryFormatter();
      Object obj = formatter.Deserialize(Stream);

      return obj;
    }

    // Метод який змінює ім'я адміністратора, приймає повідомлення в якому є id адміністратора та новe ім'я
    private void EditAdminName(string mess)
    {
      string[] adminEl = mess.Split();
      int id = int.Parse(adminEl[0]);
      string newName = adminEl[1];
      using (var db = new TestingSystemDBContext())
      {
        if (db.Administrators.Where(p => p.Login == newName).Count() == 0)
        {
          Administrator admin = db.Administrators.FirstOrDefault(a => a.Id == id);
          admin.Name = newName;
          db.SaveChanges();
          SendMessage("succesfully");
        }
        else
        {
          SendMessage("Exist admin with this name");
        }
      }
    }

    // Метод який змінює логін адміністратора, приймає повідомлення в якому є id адміністратора та новий логін
    private void EditAdminLogin(string mess)
    {
      string[] adminEl = mess.Split();
      int id = int.Parse(adminEl[0]);
      string newLogin = adminEl[1];
      using (var db = new TestingSystemDBContext())
      {
        if (db.Administrators.Where(p => p.Login == newLogin).Count() == 0)
        {
          Administrator admin = db.Administrators.FirstOrDefault(a => a.Id == id);
          admin.Login = newLogin;
          db.SaveChanges();
          SendMessage("succesfully");
        }
        else
        {
          SendMessage("Exist admin with this login");
        }
      }
    }

    // Метод який змінює пароль адміністратора, приймає повідомлення в якому є id адміністратора та новий пароль 
    private void EditAdminPassword(string mess)
    {
      string[] adminEl = mess.Split();
      int id = int.Parse(adminEl[0]);
      string newPassword = adminEl[1];
      using (var db = new TestingSystemDBContext())
      {
        if (db.Administrators.Where(p => p.Password == newPassword).Count() == 0)
        {
          Administrator admin = db.Administrators.FirstOrDefault(a => a.Id == id);
          admin.Password = newPassword;
          db.SaveChanges();
          SendMessage("succesfully");
        }
        else
        {
          SendMessage("Exist admin with this password");
        }
      }
    }

    // Метод який повертає групи студентів
    private void GetGroups()
    {
      using (var db = new TestingSystemDBContext())
      {
        List<Group> groups = db.Groups.ToList(); // список груп із бази
        List<DTOGroup> dtoGroups = new List<DTOGroup>(); // Список груп для передавання на клієнтську частину
        foreach (Group item in groups)
        {
          List<int> studentsId = new List<int>();
          List<int> testsId = new List<int>();
          foreach (Student st in item.Students)
          {
            studentsId.Add(st.StudentId);
          }
          foreach (Test ts in item.Tests)
          {
            testsId.Add(ts.TestId);
          }
          DTOGroup group = new DTOGroup { GroupId = item.GroupId, GroupName = item.GroupName, StudentsId = studentsId, TestsId = testsId }; // Об'єкт групи для передавання на клієнтську частину
          dtoGroups.Add(group); // додаємо елемент до списку
        }

        SendObject(dtoGroups); // відправляємо об'єкт клієнту
      }

    }

    // Закриває з'єднання
    protected internal void Close()
    {
      if (Stream != null)
      {
        Stream.Close();
        Stream.Dispose();
      }
      if (client != null)
      {
        client.Close();
        Stream.Dispose();
      }
    }

  }
}
