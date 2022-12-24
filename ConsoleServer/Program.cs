using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //Console.WriteLine("Привіт");
            //Console.WriteLine("Наш PC "+ Dns.GetHostName());
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //int i = 0;
            //foreach (var ip in ipHostInfo.AddressList)
            //{
            //    Console.WriteLine($"{i} - {ip}");
            //    i++;
            //}
            Console.Write("Enter IP->_");
            string ip =Console.ReadLine();
            IPAddress ip_select = IPAddress.Parse(ip);
            Console.WriteLine("Ваш сервак буде працювати IP -> "+ ip_select);
            Console.Write("Вкажіть порт(1078)->_ ");
            int port = int.Parse(Console.ReadLine());
            IPEndPoint endPoint = new IPEndPoint(ip_select, port);
            Console.Title = endPoint.ToString();

            Socket server = new Socket(ip_select.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Bind(endPoint);
                server.Listen(1000);
                while(true)
                {
                    Console.WriteLine("Очікуємо запитів від клєнтів");
                    Socket client = server.Accept(); //метод спрацьовує коли нам приходить запит
                    Console.WriteLine("Client info {0}", client.RemoteEndPoint.ToString());
                    byte[] dataClient = new byte[1024];
                    client.Receive(dataClient); //читаю дані від клієнта і зписую їх у масив
                    Console.WriteLine("Нам прийшло: {0}", Encoding.UTF8.GetString(dataClient));
                    byte[] sendBytes = new byte[1024];
                    sendBytes = Encoding.UTF8.GetBytes("Ваші дані отримано "+ DateTime.Now.ToString());
                    client.Send(sendBytes); //відпраляємо дані назад клієнту
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Виникла проблема {0}", ex.Message);
            }

        }
    }
}
