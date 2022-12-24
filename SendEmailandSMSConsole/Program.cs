using System;
using System.IO;
using System.Text;

namespace SendEmailandSMSConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            //Console.WriteLine("Відправка повідомлення novakvova@gmail.com!");
            SmtpEmailService emailService = new SmtpEmailService();
            //emailService.DownloadMessages();

            Message message = new Message();
            message.Subject = "Це прикольний шаблон";
            string html = File.ReadAllText("email-template/index.html");
            message.Body = html; //"Беріть гернератори, інвертери або свічки і усе буде круто";
            message.To = "novakvova@gmail.com";
            emailService.Send(message);

            //SMSService sms = new SMSService();
            //sms.Send("38097174909890", "Доброго вечора! Виздоровлюйте швидше. Нехай буде мирне небо. :)");

        }
    }
}
