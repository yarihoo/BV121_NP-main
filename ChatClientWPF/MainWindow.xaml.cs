using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClientWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient client = new TcpClient();
        NetworkStream ns;
        Thread thread;
        ChatMessage _message = new ChatMessage();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                _message.UserName = txtUserName.Text;
                _message.UserId = Guid.NewGuid().ToString();
                client.Connect(ip, port);
                ns = client.GetStream();
                thread = new Thread(ReciveData);
                thread.Start(client);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Проблема при роботі", ex.Message);
            }
        }
        //Оримуємо дані від сервера
        private void ReciveData(object o)
        {
            TcpClient client = (TcpClient)o;
            NetworkStream ns = client.GetStream();
            var reciveBytes = new byte[4096];
            int byte_count;
            
            while((byte_count=ns.Read(reciveBytes))>0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    string data = Encoding.UTF8.GetString(reciveBytes);
                    ChatMessage message = ChatMessage.Dessialize(reciveBytes);
                    lbInfo.Items.Add(message.UserName + "-> "+ message.Text);
                }));
            }
        }

        private void bntSend_Click(object sender, RoutedEventArgs e)
        {
            _message.Text = txtText.Text;
            var buffer = _message.Serialize();
            ns.Write(buffer);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
