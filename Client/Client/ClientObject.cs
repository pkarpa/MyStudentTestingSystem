using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TestingSystemDatabaseLibrary;
using System.Runtime.Serialization;
using DTOLibraryTestingSystem;

namespace Client
{
    // Клас клієнта
    public class ClientObject
    {
        private const string host = "127.0.0.1"; // ip-адреса певного комп'ютера
        private const int port = 8888; // port по якому буде встановлюватися з'єднання
        static TcpClient client; // об'єкт клієнта
        static NetworkStream stream; // потік по якому будуть передаватися повідомлення
        static int id; // id клієнта 
        public bool IsAdmin { get; set; } // чи є він адміном

        public void Connect()
        {
            client = new TcpClient();

            try
            {
                client.Connect(host, port); // встонавлюємо з'єднання з сервером
                stream = client.GetStream(); // отримуємо потік для передачі і прийому повідомлень
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Метод який надсилає об'єкти на серверну частину
        private void SendObject(Object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        // Метод який надсилає повідомлення типу String на серверну частину
        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);// перетворюємо повідомлення типу String в масив байт
            stream.Write(data, 0, data.Length); // Відправдяємо дані 
        }

        // Метод який приймає повідомлення з серверної частини
        public string RecieveMessages()
        {
            string message = null;
            try
            {
                byte[] data = new byte[64]; // масив для зберігання даних з потоку
                StringBuilder builder = new StringBuilder(); // об'єкт для зберігання повідомлення 
                int bytes = 0;

                // Зчитуємо дані з потоку, поки вони там є 
                do
                {
                    bytes = stream.Read(data, 0, data.Length); // розмір даних які були додані в масив data
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes)); // витягуємо дані з масиву байт data, перетворююси їх в String 
                }
                while (stream.DataAvailable);

                message = builder.ToString();
            }
            catch
            {
                MessageBox.Show("Lose Connection!");
                Disconnect();
                Environment.Exit(0);
            }

            return message; // Повертаємо отримане повідомлення
        }

        // Метод який приймає об'єкти з серверної частини
        public Object RecieveObject()
        {
            Object obj = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(stream);
            }
            catch
            {
                MessageBox.Show("Lose Connection!");
                Disconnect();
                Environment.Exit(0);
            }
            return obj;
        }

        public bool IsConnected()
        {
            bool isConnect = false;
            if (client.Connected == true)
            {
                isConnect = true;
            }
            return isConnect;
        }

        // Закриває з'єднання
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            //Environment.Exit(0);
        }

        // Реєстрація користувача, приймає об'єкт який може бути DTOAdministrator або DTOStudent
        public string Registrate(Object obj)
        {
            string message = null;

            if (obj is DTOAdministrator) // Перевіряємо чи об'єкт це DTOAdministrator
            {
                message = "AdminRegistration:";
                SendMessage(message); // надсилаємо повідомлення на серверну частину про реєстрацію адміністратора 
                SendObject(obj); // надсилаємо об'єкт адміністратора з даними  на серверну частину
            }
            else
            {
                message = "StudentRegistration:";
                SendMessage(message); // надсилаємо повідомлення на серверну частину про реєстрацію студента 
                SendObject(obj); // надсилаємо об'єкт студента з даними  на серверну частину
            }

            string answer = RecieveMessages(); // Получаємо відповідь з сервера

            return answer;
        }

        // Метод який виконує вхід певного користувача в систему
        // Приймає ідентифікатор, який вказує чи є користувач адміністратором, його логін та пароль
        public string LogIn(string login, string password)
        {
            string message = "LogIn:" + login + " " + password;
            SendMessage(message);  // надсилаємо повідомлення на серверну частину про вхід користувача з даними  
            string[] answer = RecieveMessages().Split(':'); // приймаємо відповідь з сервера чи існує клієнт з такими даними 
            if (answer.Length > 1)
            {
                string[] el = answer[0].Split();
                id = int.Parse(el[0]);// получаємо id клієнта
                IsAdmin = bool.Parse(el[1]); // чи це адмін
                return answer[1]; // повертаємо відповідь
            }
            else
            {
                return answer[0];
            }

        }

        //Метод зміни імені адміністратора, приймає нове ім'я
        public string EditAdminName(string name)
        {
            string message = "EditAdminName:" + id + " " + name;
            string answer = "";
            SendMessage(message); // надсилаємо повідомлення на серверну частину про зміну імені адміністратора
            answer = RecieveMessages();  // приймаємо відповідь з сервера 
            return answer; // повертаємо відповідь
        }

        //Метод зміни логіну адміністратора, приймає новий логін
        public string EditAdminLogin(string login)
        {
            string message = "EditAdminLogin:" + id + " " + login;
            string answer = "";
            SendMessage(message); // надсилаємо повідомлення на серверну частину про зміну логіну адміністратора
            answer = RecieveMessages();  // приймаємо відповідь з сервера 
            return answer; // повертаємо відповідь
        }

        //Метод зміни паролю адміністратора, приймає новий пароль
        public string EditAdminPassword(string password)
        {
            string message = "EditAdminPassword:" + id + " " + password;
            string answer = "";
            SendMessage(message); // надсилаємо повідомлення на серверну частину про зміну паролю адміністратора
            answer = RecieveMessages();  // приймаємо відповідь з сервера 
            return answer; // повертаємо відповідь
        }

        // Метод який повертає групи студентів 
        public List<DTOGroup> GetGroups()
        {
            string message = "GetGroups:";
            SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
            List<DTOGroup> groups = (List<DTOGroup>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
            return groups;
        }

        // Метод який повертає групи студентів для конкретного адміністратора
        public List<DTOGroup> GetGroupsForAdmin()
        {
            string message = "GetGroupsForAdmin:" + id;
            SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
            List<DTOGroup> groups = (List<DTOGroup>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
            return groups;
        }

        // Метод який видаляє групу за її id
        public string DeleteGroup(int id)
        {
            string message = "DeleteGroup:" + id;
            SendMessage(message); // надсилаємо повідомлення на серверну частину про видалення групи
            string answer = RecieveMessages(); // приймаємо відповідь з сервера

            return answer;
        }

        // Метод який повертає студентів певної групи, приймає список id студентів групи
        public List<DTOStudent> GetGroupStudents(List<int> studentsId)
        {
            string message = "GetGroupStudents:";
            SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання студентів
            SendObject(studentsId);  // надсилаємо список id студентів групи
            List<DTOStudent> students = new List<DTOStudent>(); // список студентів групи         
            var obj = RecieveObject();  // приймаємо відповідь з сервера
            if (obj is List<DTOStudent>) // перевіряємо чи це список студентів
            {
                students = obj as List<DTOStudent>; // приводимо до потрібного типу
            }

            return students;
        }

        // Метод який повертає тести, які доступні для певної групи, приймає список id тестів групи
        public List<DTOTest> GetGroupTests(List<int> testsId)
        {
            string message = "GetGroupTests:";
            SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання тестів
            SendObject(testsId); // надсилаємо список id тестів групи
            List<DTOTest> tests = new List<DTOTest>(); // список тестів групи
            var obj = RecieveObject();// приймаємо відповідь з сервера       
            if (obj is List<DTOTest>) // перевіряємо чи це список тестів
            {
                tests = obj as List<DTOTest>; // приводимо до потрібного типу
            }

            return tests;
        }

        // Метод який видаляє студента з групи за його id
        public string DeleteStudentFromGroup(int id)
        {
            string mess = "DeleteStudentFromGroup:" + id;
            SendMessage(mess); // надсилаємо повідомлення на серверну частину про видалення студента з групи
            string answer = RecieveMessages(); // приймаємо відповідь з сервера 

            return answer;
        }

        public void LogOut()
        {
            string message = "LogOut:";
            SendMessage(message);
            this.Disconnect();
        }

        // Метод для створення нової групи студентів, приймає назву групи
        public string AddGroup(string name)
        {
            string message = "AddGroup:" + id + " " + name;
            SendMessage(message); // надсилаємо повідомлення на серверну частину про створення нової групи
            string answer = RecieveMessages();  // приймаємо відповідь з сервера 

            return answer;
        }

        // Метод який повертає інформацію про адміністратора
        public DTOAdministrator GetInfoAboutAdmin()
        {
            string message = "InfoAboutAdmin:";
            SendMessage(message + id); // надсилаємо повідомлення на серверну частину для того щоб отримати інформацію про адміністратора
            var obj = RecieveObject();  // приймаємо відповідь з сервера 
            DTOAdministrator administrator = null;
            if (obj is DTOAdministrator) // перевіряємо чи це справді DTOAdministrator
            {
                administrator = (DTOAdministrator)obj; // приводимо до потрібного типу
            }

            return administrator;
        }

        // Метод який повертає інформацію про студента
        public DTOStudent GetInfoAboutStudent()
        {
            string message = "InfoAboutStudent:";
            SendMessage(message + id); // надсилаємо повідомлення на серверну частину для того щоб отримати інформацію про адміністратора
            var obj = RecieveObject();  // приймаємо відповідь з сервера 
            DTOStudent student = null;
            if (obj is DTOStudent) // перевіряємо чи це справді DTOAdministrator
            {
                student = (DTOStudent)obj; // приводимо до потрібного типу
            }

            return student;
        }
    }
}
