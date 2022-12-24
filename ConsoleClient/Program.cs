using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Вкажіть ip сервера:");
            IPAddress ip = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Вкажіть порт сервера:");
            int port = int.Parse(Console.ReadLine());
            IPEndPoint serverEndPoint = new IPEndPoint(ip, port);
            Socket sender = new Socket(ip.AddressFamily,
                SocketType.Stream, 
                ProtocolType.Tcp);
            try
            {
                sender.Connect(serverEndPoint); //клієнта підключаємо до сервера
                byte[] buffer = Encoding.UTF8.GetBytes("Привіт наш хороший. У тебе світло є?");
                sender.Send(buffer); //буфер литить на сервер
                byte[] serverResponce = new byte[1024]; //масив зберігає відповідь від сервера
                sender.Receive(serverResponce); //отримали дані від сервера
                Console.WriteLine("Сервер каже: {0}", Encoding.UTF8.GetString(serverResponce));
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Викли проблеми при роботі із серверо{0}", ex.Message);
            }
        }
    }
}
