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
                        case "GetGroupsForAdmin":
                            GetGroupsForAdmin(message[1]);
                            Console.WriteLine(this.ClientObjectId + ": GetGroupsForAdmin");
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
                        case "DeleteStudentFromGroup":
                            DeleteStudentFromGroup(message[1]);
                            Console.WriteLine(this.ClientObjectId + ": DeleteStudentFromGroup");
                            break;
                        case "InfoAboutStudent":
                            InfoAboutStudent(message[1]);
                            Console.WriteLine(this.ClientObjectId + ": InfoAboutStudent");
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
                server.RemoveConnection(this.ClientObjectId);
                Close();
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
            isAdmin = true;
            login = logEl[0];
            password = logEl[1];

            using (var db = new TestingSystemDBContext())
            {
                if (db.Administrators.FirstOrDefault(a => a.Login == login) == null)
                {
                    if (db.Administrators.FirstOrDefault(a => a.Password == password) == null)
                    {
                        isAdmin = false;
                    }
                }

                if (isAdmin)
                {
                    if (db.Administrators.FirstOrDefault(a => a.Login == login) != null)
                    {
                        if (db.Administrators.FirstOrDefault(a => a.Password == password) != null)
                        {
                            var admin = db.Administrators.FirstOrDefault(a => a.Password == password);
                            SendMessage(admin.Id + " " + isAdmin + ":succesfully");
                        }
                        else
                        {
                            SendMessage("No user with this password");
                        }
                    }
                    else
                    {
                        SendMessage("No user with this login");
                    }
                }
                else
                {
                    if (db.Students.FirstOrDefault(s => s.Login == login) != null)
                    {
                        if (db.Students.FirstOrDefault(a => a.Password == password) != null)
                        {
                            var student = db.Students.FirstOrDefault(s => s.Password == password);
                            SendMessage(student.StudentId + " " + isAdmin + ":succesfully");
                        }
                        else
                        {
                            SendMessage("No user with this password");
                        }
                    }
                    else
                    {
                        SendMessage("No user with this login");
                    }
                }
            }
        }

        private void LogOut()
        {
            server.RemoveConnection(ClientObjectId);
            this.Close();
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
                    if (db.Administrators.FirstOrDefault(a => a.Password == dtoAdmin.Password) == null)
                    {
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
                    else
                    {
                        SendMessage("password already exists");
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
                    if (db.Students.FirstOrDefault(s => s.Password == dtoStudent.Password) == null)
                    {
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
                    else
                    {
                        SendMessage("password already exists");
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
            if (obj is List<int>)
            {
                List<DTOStudent> students = new List<DTOStudent>();// Список студентів
                List<int> studentsId = (List<int>)obj; // Приведення до потрібного типу
                using (var db = new TestingSystemDBContext())
                {
                    for (int i = 0; i < studentsId.Count(); i++)
                    {
                        int id = studentsId[i];
                        Student student = db.Students.Include(g => g.Group).FirstOrDefault(s => s.StudentId == id); // Отримуємо студента з бази по його id
                        DTOStudent dtoStudent = new DTOStudent { StudentId = student.StudentId, Name = student.Name, SurName = student.SurName, Login = student.Login, Password = student.Password, GroupId = student.Group.GroupId }; // Створюємо об'єкт студента для надсилання
                        students.Add(dtoStudent); // Додаємо студента до списку 
                    }
                }

                SendObject(students); // Надсилаємо список студентів на клієнтську частину
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


        // Метод який повертає інформацію про певного адміністратора
        private void InfoAboutAdmin(string mess)
        {
            int adminId = int.Parse(mess); // отримуємо id адміністратора з повідомлення
            using (var db = new TestingSystemDBContext())
            {
                var admin = db.Administrators.FirstOrDefault(a => a.Id == adminId); // Отримуємо адміністратора з бази по id
                List<int> groupsId = new List<int>(); // список груп, які створив цей адміністратор
                List<int> subjectsId = new List<int>(); // список предметів, які створив цей адміністратор
                if (admin.Groups.Count != 0)
                {
                    foreach (var item in admin.Groups)
                    {
                        groupsId.Add(item.GroupId);
                    }
                }
                if (admin.Subjects.Count != 0)
                {
                    foreach (var item in admin.Subjects)
                    {
                        groupsId.Add(item.SubjectId);
                    }
                }
                DTOAdministrator administrator = new DTOAdministrator { Id = admin.Id, Name = admin.Name, Login = admin.Login, Password = admin.Password, GroupsId = groupsId, SubjectsId = subjectsId }; // створення об'єкта адміністратора для надсилання 
                SendObject(administrator); // надсилання об'єкта адміністратора на клієнтську частину
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
                DTOStudent dtoStudent = new DTOStudent { StudentId = student.StudentId, Name = student.Name, SurName = student.SurName, Login = student.Login, Password = student.Password, GroupId = student.Group.GroupId, GroupName = student.Group.GroupName, TestSessionsId = testSessionsId }; // створення об'єкта студента для надсилання 
                SendObject(dtoStudent); // надсилання об'єкта студента на клієнтську частину
            }
        }

        // Створення та збереження нової групи студентів в базу 
        private void AddGroup(string mess)
        {
            string[] el = mess.Split(); // отримуємо повіжомлення з даними про групу
            int adminId = int.Parse(el[0]); // id адміністратора який створив групу
            string groupName = el[1]; // назва групи
            using (var db = new TestingSystemDBContext())
            {
                // перевірка чи не існує вже групи з таким іменем в базі 
                if (db.Groups.FirstOrDefault(g => g.GroupName == groupName) == null)
                {
                    Group gr = new Group() { Admin = db.Administrators.FirstOrDefault(a => a.Id == adminId), GroupName = groupName }; // Створення об'єкта групи
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
                List<Group> groups = db.Groups.Include(p => p.Admin).ToList(); // список груп із бази
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
                    DTOGroup group = new DTOGroup { GroupId = item.GroupId, AdminId = item.Admin.Id, GroupName = item.GroupName, StudentsId = studentsId, TestsId = testsId }; // Об'єкт групи для передавання на клієнтську частину
                    dtoGroups.Add(group); // додаємо елемент до списку
                }

                SendObject(dtoGroups); // відправляємо об'єкт клієнту
            }

        }

        // Метод який повертає групи студентів для певного адміністратора
        private void GetGroupsForAdmin(string mess)
        {
            int id = int.Parse(mess);
            using (var db = new TestingSystemDBContext())
            {
                List<Group> groups = db.Groups.Include(p => p.Admin).Where(a => a.Admin.Id == id).ToList(); // список груп із бази
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
                    DTOGroup group = new DTOGroup { GroupId = item.GroupId, AdminId = item.Admin.Id, GroupName = item.GroupName, StudentsId = studentsId, TestsId = testsId }; // Об'єкт групи для передавання на клієнтську частину
                    dtoGroups.Add(group); // додаємо елемент до списку
                }

                SendObject(dtoGroups); // відправляємо об'єкт клієнту
            }

        }

        // Закриває з'єднання
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }

    }
}
