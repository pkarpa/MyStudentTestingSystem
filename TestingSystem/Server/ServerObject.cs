﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace TestingSystemServer
{
    // клас сервера
    public class ServerObject
    {
        static TcpListener tcpListener; // об'єкт який приймає вхідні з'єднання
        List<ClientObject> clients = new List<ClientObject>(); // Список який містить всі з'єднання

        // Метод який додає нове з'єднання
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        // Метод який видаляє з'єднання
        protected internal void RemoveConnection(string id)
        {
            ClientObject client = clients.FirstOrDefault(c => c.ClientObjectId == id);
            if (client != null)
                clients.Remove(client);
        }

        protected internal void RemoveConnections()
        {
           clients.Clear();
        }


    // Метод прослуховеє вхідні з'єднання
    protected internal void Listen()
    {
      try
      {
        tcpListener = new TcpListener(IPAddress.Any, 8888); // Встановлюємо ip-адресу та потр за яким будемо прослуховувати вхідні з'єднання
        tcpListener.Start(); // Починаємо прослуховування
        Console.WriteLine("Start Server...");

        while (true)
        {
          TcpClient tcpClient = tcpListener.AcceptTcpClient(); // Приймаємо вхідне з'єднання
          ClientObject clientObject = new ClientObject(tcpClient, this); // Створюємо новий об'єкт клієнта
          Thread clientThread = new Thread(new ThreadStart(clientObject.Process)); // Запускаємо метод Process в новому потоці
          clientThread.Start();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        Disconnect();
      }


      //try
      //{
      //  TcpClient client = await listener.AcceptTcpClientAsync();
      //  var clientTask = protocol.HandleClient(client, cancellationToken)
      //      .ContinueWith((antecedent) => client.Dispose())
      //      .ContinueWith((antecedent) => logger.LogInformation("Client disposed."));
      //}

    }

        // Зупиняє прослуховування вхідних з'єднань
        protected internal void Disconnect()
        {
            tcpListener.Stop();

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            Environment.Exit(0); 
        }
    }
}
