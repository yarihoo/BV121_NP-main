using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServerConsole
{
    class Program
    {
        //блокування потоку для коректної роботи чата.
        static readonly object _lock = new object();
        //список клієнтів, які є в чаті.
        static readonly Dictionary<int, TcpClient> list_clients = 
            new Dictionary<int, TcpClient>();
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            int count = 1;
            string fileName = "config.txt";
            IPAddress ip;
            int port;
            using(FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    ip = IPAddress.Parse(sr.ReadLine());
                    port = int.Parse(sr.ReadLine());
                }
            }
            TcpListener serverSocket = new TcpListener(ip, port);
            serverSocket.Start();
            Console.WriteLine("Запуск сервака {0}:{1}",ip,port);
            while(true)
            {
                //сервер очікує підключення клієнтів
                TcpClient client = serverSocket.AcceptTcpClient();
                lock(_lock) { list_clients.Add(count, client); }
                Console.WriteLine("Появився на сервері новий клієнт {0}", client.Client.RemoteEndPoint);
                Thread t = new Thread(handle_clients);
                t.Start(count); //Окремий потік у якому будемо спілкувати із даним клієнтом
                count++;
            }
        }
        public static void handle_clients(object c)
        {
            int id = (int)c;
            TcpClient client;
            lock(_lock) { client = list_clients[id]; }
            while(true) // у цьому циклі ми будемо оримувати дані від окремо клієнта
            {
                NetworkStream stream = client.GetStream(); //отримали потік окремого клієнта, який підключвися на сервак
                byte[] buffer = new byte[4096]; //щоб якщо буде велике повідомлення, то має влізти
                int byte_count = stream.Read(buffer); //прочиталі дані від клієнта
                if (byte_count == 0) break; //якщо клієнта пислав пустоту, то ми зним прощаємося
                string data = Encoding.UTF8.GetString(buffer, 0, byte_count); //отримали текстове повідомлення від клієнта
                broadcast(data); //розіслали повідомлення усім клєінта хто є в чаті.
                Console.WriteLine(data); //показуємо повідомення, що прислав клієнт
            }
            lock(_lock) { list_clients.Remove(id); }
            Console.WriteLine("Покинув чат клієнт");
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        //Відправляємо повідомлення усім клієнтам, які є в чаті - метод broadcast
        public static void broadcast(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            
            lock(_lock) //Щоб повідомлення при розсилці не перемішувалися
            {
                Console.WriteLine("Count Clients: {0}", list_clients.Values.Count);
                foreach(TcpClient c in list_clients.Values) //перебираємо усі клієнтів
                {
                    NetworkStream stream = c.GetStream(); //отримуємо потік окремого клієнта
                    stream.Write(buffer); //відправляємо повідомлення
                }
            }
        }
    }
}
