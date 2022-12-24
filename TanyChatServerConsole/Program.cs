using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TanyChatServerConsole
{
    internal class Program
    {
        //блокування потоку для коректної робти чата
        static readonly object _lock = new object();
        //список клієнтів, які є в чаті
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();
        static void Main(string[] args)
        {

            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            int count = 1; //ксть клієнтів які є

            string fileName = "config.txt";
            IPAddress ip;
            int port;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    ip = IPAddress.Parse(sr.ReadLine());
                    port = int.Parse(sr.ReadLine());
                }
            }
            TcpListener serverSocket = new TcpListener(ip, port);  //запускаємо сервер
            serverSocket.Start();
            Console.WriteLine("Запуск сервака {0}:{1}", ip, port);

            while (true)
            {
                //сервер очікує підключення клієнтів
                TcpClient client = serverSocket.AcceptTcpClient();
                lock (_lock)
                {
                    list_clients.Add(count, client); //зупиняємо основний потік, щоб безпечно додати клієнта, щоб не всі зразу
                }
                Console.WriteLine("Появився на сервері новий клієнт {0}", client.Client.RemoteEndPoint); //хто з клієнтів підключився

                Thread t = new Thread(handle_clients);  //асинхронно,  окремий потік для клієнта кожного, де ми будемо cпілкуватися із даним клієнтом
                t.Start(count);

                count++;
            }


        }

        public static void handle_clients(object c)
        {
            int id = (int)c;
            TcpClient client;
            lock (_lock)
            {
                client = list_clients[id];
            }
            while(true) // тут отримуємо дані від окемого клієнта, постійно чекаємо дані від клєнтів
            {
                NetworkStream stream = client.GetStream(); //отримали потік окремого клієнта, який підключився на сервер
                byte[] buffer = new byte[16054400];
                
                int byte_count = stream.Read(buffer); // читаємо дані від клієнта
                if (byte_count == 0)
                    break; //якщо клієнт надіслав пусте повідомлення, то прощаємся

                string data = Encoding.UTF8.GetString(buffer, 0, byte_count); //отримали текстове повідомлення від клієнта

                broadcast(data); // розсилаємо повідомення усім
                //Console.WriteLine(data); //показуємо повідомлення, яке прислав клієнт
            }
            lock(_lock)
            {
                Console.WriteLine("Клієнт {0} покидає чат", client.Client.RemoteEndPoint);
                list_clients.Remove(id); //коли клієт покидає чат
            }
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        public static void broadcast(string data) // розсилати повідомення всім клієнтам які є в чаті
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            lock (_lock) //щоб повідомлення не переміщувались, а й шли по черзі. поки одне повідомлення не надішлеться, то інше не піде
            {
                foreach (TcpClient c in list_clients.Values) //перебираємо всіх клієнтів
                {
                    NetworkStream stream = c.GetStream(); //отримємо потік окремого клієнта
                    stream.Write(buffer); //відправляємо повідомлення
                }
            }
        }
    }
}
