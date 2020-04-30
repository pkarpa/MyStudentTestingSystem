using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestingSystemServer
{
    class Program
    {
        static ServerObject server; 
        static Thread listenThread; 
        static void Main(string[] args)
        {
            try
            {
                server = new ServerObject(); // Створюємо новий об'єкт сервера
                listenThread = new Thread(new ThreadStart(server.Listen)); // Створюємо новий потік в якому запускаємо метод Listen сервера
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
