﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using DTO;

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

    public int GetClientId()
    {
        return id;
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
      {
        stream.Dispose();
        stream.Close();
      }

      if (client != null)
      {
        client.Close();
        client.Dispose();
      }


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

    public int AddTestSession(DTOTestSession testSession)
    {
            string message = "AddTestSession:";
            SendMessage(message);
            Thread.Sleep(1000);
            SendObject(testSession);
            Thread.Sleep(1000);
            string answer = RecieveMessages();
            int testSessionId = int.Parse(answer);
            return testSessionId;
    }
        
    public List<int> AddStudentsAnswers(List<DTOAnswer> _userAnswers)
    {
        string message = "AddStudentsAnswers:";
        SendMessage(message);
        Thread.Sleep(300);
        SendObject(_userAnswers);
        Thread.Sleep(700);
        var answer = RecieveMessages().Split();
        Thread.Sleep(1000);
        List<int> answersId = new List<int>();
        foreach (var item in answer)
        {
            answersId.Add(int.Parse(item));
        }

        return answersId;
    }
        
    public string AddSubject(string subjectName)
    {
        string message = "AddSubject:" + id + " " + subjectName;
        SendMessage(message);
        Thread.Sleep(700);
        string answer = RecieveMessages();
        return answer;
    }

    public string AddTheme(int subjectId, string themeName)
    {
        string message = "AddTheme:" + subjectId + "_" + themeName;
        SendMessage(message);
        Thread.Sleep(700);
        string answer = RecieveMessages();
        return answer;
    }
        
    public void DeleteTheme(int themeId)
    {
        string message = "DeleteTheme:" + themeId;
        SendMessage(message);
        Thread.Sleep(3000);
    }
        

    public void DeleteTest(int testId)
    {
        string message = "DeleteTest:" + testId;
        SendMessage(message);
        Thread.Sleep(3000);
    }
    public void DeleteSubject(int subjectId)
    {
        string message = "DeleteSubject:" + subjectId;
        SendMessage(message);
        Thread.Sleep(3000);
    }

    // Метод який повертає групи студентів 
    public List<DTOGroup> GetGroups()
    {
      string message = "GetGroups:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
      List<DTOGroup> groups = (List<DTOGroup>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
      return groups;
    }

    public string GetTestName(int testId)
    {
        string message = "GetTestName:" + testId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        string answer = RecieveMessages();
        return answer;
    }

    public List<DTOStudent> GetStudents()
    {
        string message = "GetStudents:";
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        List<DTOStudent> students = (List<DTOStudent>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return students;
    }

    public void AddTest(DTOTest test)
    {
        string message = "AddTest:";
        SendMessage(message);
        Thread.Sleep(700);
        SendObject(test);
    }


    public List<DTOAnswer> GetAnswersForTestSession(int testSessionId)
    {
        string message = "GetAnswersForTestSession:" + testSessionId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<DTOAnswer> answers = (List<DTOAnswer>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return answers;
    }

    public List<DTOTestSession> GetTestSessionsForSomeStudent()
    {
        string message = "GetTestSessionsForSomeStudent:" + id;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<DTOTestSession> answers = (List<DTOTestSession>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return answers;
    }

    public List<QueDTOQuestionstion> GetQuestionsWithoutParams()
    {
        string message = "GetQuestionsWithoutParams:";
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<QueDTOQuestionstion> questions = (List<QueDTOQuestionstion>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return questions;
    }

    public List<DTOTestSession> GetTestSessionsForSomeTest(int testId)
    {
        List<DTOTestSession> dtoTestSessions = new List<DTOTestSession> ();
        string message = "GetTestSessionsForNeededTest:" + testId;
        SendMessage(message);
        Thread.Sleep(700);
        var obj = RecieveObject();
        Thread.Sleep(700);
        if (obj is List<DTOTestSession>)
        {
            dtoTestSessions = obj as List<DTOTestSession>;
        }
        return dtoTestSessions;
    }

    // Метод який повертає групи студентів 
    public List<DTOSubject> GetSubjects()
    {
      string message = "GetSubjects:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
      Thread.Sleep(700);
      List<DTOSubject> groups = (List<DTOSubject>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
      return groups;
    }
        
    public List<DTOSubject> GetSubjectsForAdmin()
    {
        string message = "GetSubjectsForAdmin:" + id;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<DTOSubject> subjects = (List<DTOSubject>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return subjects;
    }
        
    public List<DTOTheme> GetThemesForSubject(int subjectId)
    {
        string message = "GetThemesForSubject:" + subjectId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<DTOTheme> themes = (List<DTOTheme>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return themes;
    }

    public List<DTOTest> GetTestsForTheme(int themeId)
    {
        string message = "GetTestsForTheme:" + themeId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання груп
        Thread.Sleep(700);
        List<DTOTest> tests = (List<DTOTest>)RecieveObject(); // приймаємо відповідь з сервера в вигляді списку груп
        return tests;
    }

        // Метод який видаляє групу за її id
    public string DeleteGroup(int id)
    {
      string message = "DeleteGroup:" + id;
      SendMessage(message); // надсилаємо повідомлення на серверну частину про видалення групи
      Thread.Sleep(700);
      string answer = RecieveMessages(); // приймаємо відповідь з сервера

      return answer;
    }

    // Метод який повертає студентів певної групи, приймає список id студентів групи
    public List<DTOStudent> GetGroupStudents(int groupId)
    {
      string message = "GetGroupStudents:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання студентів
      Thread.Sleep(700);
      SendObject(groupId);  // надсилаємо список id студентів групи
      Thread.Sleep(700);
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
        Thread.Sleep(700);
        SendObject(testsId); // надсилаємо список id тестів групи
      List<DTOTest> tests = new List<DTOTest>(); // список тестів групи
        Thread.Sleep(700);
        var obj = RecieveObject();// приймаємо відповідь з сервера       
      if (obj is List<DTOTest>) // перевіряємо чи це список тестів
      {
        tests = obj as List<DTOTest>; // приводимо до потрібного типу
      }

      return tests;
    }

    public List<DTOTest> GetListAvaibleTests(int groupId, int themeId)
    {
        string message = "GetListAvaibleTests:" + groupId + " " + themeId;
        SendMessage(message);
        Thread.Sleep(700);
        List<DTOTest> avaibleTests = new List<DTOTest>(); // список студентів групи         
        var obj = RecieveObject();  // приймаємо відповідь з сервера
        if (obj is List<DTOTest>) // перевіряємо чи це список студентів
        {
            avaibleTests = obj as List<DTOTest>; // приводимо до потрібного типу
        }

        return avaibleTests;
    }

    public List<QueDTOQuestionstion> GetQuestions(int testsId)
    {
      string message = "GetQuestions:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання 
      Thread.Sleep(700);
      SendObject(testsId); // надсилаємо список id тестів групи
      List<QueDTOQuestionstion> questions = new List<QueDTOQuestionstion>(); // список 
      Thread.Sleep(700);
      var obj = RecieveObject();// приймаємо відповідь з сервера       
      if (obj is List<QueDTOQuestionstion>) // перевіряємо чи це список тестів
      {
        questions = obj as List<QueDTOQuestionstion>; // приводимо до потрібного типу
      }

      return questions;
    }

    public string AddQuestionIdInTest(int testId, int questionId)
    {
        string message = "AddQuestionIdInTest:" + testId + " " + questionId;
        Thread.Sleep(1010);
        SendMessage(message);
        Thread.Sleep(1010);
        string answer = RecieveMessages();
            return answer;
    }

    public int SaveQuestion(QueDTOQuestionstion question)
    {
      string message = "SaveQuestion:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання 
      Thread.Sleep(1010);
      SendObject(question); // надсилаємо список id тестів групи
      Thread.Sleep(1010);
      var obj = RecieveObject();// приймаємо відповідь з сервера       
      int newId = 0;
      if (obj is int) // перевіряємо чи це список тестів
      {
        return (int)obj; // приводимо до потрібного типу
      }

      return newId;
    }

    public bool UpdateQuestion(QueDTOQuestionstion question)
    {
      string message = "UpdateQuestion:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання 
      Thread.Sleep(700);
      SendObject(question); // надсилаємо список id тестів групи
      Thread.Sleep(700);
      var obj = RecieveObject();// приймаємо відповідь з сервера   
      bool result = false;
      if (obj is bool) // перевіряємо чи це список тестів
      {
        result = (bool)obj; // приводимо до потрібного типу
      }

      return result;
    }

    public void DeleteQuestion(int questionId)
    {
      string message = "DeleteQuestion:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання 
      Thread.Sleep(700);
      SendObject(questionId); // надсилаємо список id тестів групи
      Thread.Sleep(700);
    }

    public void UpdateTestSession(DTOTestSession _testSession)
    {
        string message = "UpdateTestSession:";
        this.SendMessage(message);
        Thread.Sleep(300);
        this.SendObject(_testSession);
        Thread.Sleep(700);
    }

    public List<DTOQuestionType> GetQuestionTypes()
    {
      string message = "GetQuestionTypes:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання 
      List<DTOQuestionType> questionsTypes = new List<DTOQuestionType>(); // список 
      Thread.Sleep(700);
      var obj = RecieveObject();// приймаємо відповідь з сервера       
      if (obj is List<DTOQuestionType>) // перевіряємо чи це список тестів
      {
        questionsTypes = obj as List<DTOQuestionType>; // приводимо до потрібного типу
      }

      return questionsTypes;
    }

    // Метод який повертає тести, які доступні для певної групи, приймає список id тестів групи
    public List<DTOTest> GetAllTests()
    {
      string message = "GetAllTests:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання тестів
      List<DTOTest> tests = new List<DTOTest>(); // список тестів групи
      Thread.Sleep(700);
      var obj = RecieveObject();// приймаємо відповідь з сервера       
      if (obj is List<DTOTest>) // перевіряємо чи це список тестів
      {
        tests = obj as List<DTOTest>; // приводимо до потрібного типу
      }

      return tests;
    }

    public DTOTest GetTest(int testId)
    {
        DTOTest dtoTest = null ;
        string message = "GetTest:" + testId;
        SendMessage(message);
        Thread.Sleep(700);
        var obj = RecieveObject();
        if(obj is DTOTest)
        {
            dtoTest = obj as DTOTest;
        }
        return dtoTest;
    }

    public List<DTOTheme> GetTheme()
    {
      string message = "GetTheme:";
      SendMessage(message); // надсилаємо повідомлення на серверну частину для отримання тестів
      Thread.Sleep(700);
      List<DTOTheme> theme = new List<DTOTheme>(); // список тестів групи
      var obj = RecieveObject();// приймаємо відповідь з сервера       
      if (obj is List<DTOTheme>) // перевіряємо чи це список тестів
      {
        theme = obj as List<DTOTheme>; // приводимо до потрібного типу
      }

      return theme;
    }

    public List<DTOTest> GetTestsForSomeGroup()
    {
        string message = "GetTestsForSomeGroup:" + id;
        SendMessage(message);
        Thread.Sleep(700);
        List<DTOTest> tests = new List<DTOTest>(); // список тестів групи
        var obj = RecieveObject();// приймаємо відповідь з сервера       
        if (obj is List<DTOTest>) // перевіряємо чи це список тестів
        {
            tests = obj as List<DTOTest>; // приводимо до потрібного типу
        }

        return tests;
    }

    public List<DTOTest> GetTestsForSomeAdmin()
    {
        string message = "GetTestsForSomeAdmin:" + id;
        SendMessage(message);
        Thread.Sleep(700);
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
      Thread.Sleep(700);
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
      string message = "AddGroup:" + name;
      SendMessage(message); // надсилаємо повідомлення на серверну частину про створення нової групи
      Thread.Sleep(700);
      string answer = RecieveMessages();  // приймаємо відповідь з сервера 

      return answer;
    }
        
    public string AddStudentToGroup(int studentId, int groupId)
    {
        string message = "AddStudentToGroup:" + groupId + " " + studentId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину про створення нової групи
        Thread.Sleep(700);
        string answer = RecieveMessages();  // приймаємо відповідь з сервера 
        return answer;
    }

    public string DeleteTestFromGroup(int testId, int groupId)
    {
        string message = "DeleteTestFromGroup:" + groupId + " " + testId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину про створення нової групи
        Thread.Sleep(700);
        string answer = RecieveMessages();  // приймаємо відповідь з сервера 
        return answer;
    }

    public string EditStudent(DTOStudent student)
    {
         string message = "EditStudent:";
         SendMessage(message);
         Thread.Sleep(700);
         SendObject(student);
         Thread.Sleep(700);
         string answer = RecieveMessages();
         return answer;
    }

    public void DeleteStudent(int studentId)
    {
      string message = "DeleteStudent:";
      SendMessage(message);
      Thread.Sleep(700);
      SendObject(studentId);
      Thread.Sleep(700);
    }

    public string AddTestToGroup(int testId, int groupId)
    {
        string message = "AddTestToGroup:" + groupId + " " + testId;
        SendMessage(message); // надсилаємо повідомлення на серверну частину про створення нової групи
        Thread.Sleep(700);
        string answer = RecieveMessages();  // приймаємо відповідь з сервера 
        return answer;
    }

        // Метод який повертає інформацію про адміністратора
    public DTOAdministrator GetInfoAboutAdmin()
    {
      string message = "InfoAboutAdmin:";
      SendMessage(message + id); // надсилаємо повідомлення на серверну частину для того щоб отримати інформацію про адміністратора
      Thread.Sleep(700);
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
      Thread.Sleep(700);
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
