using MailKit.Net.Imap;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;

namespace SendEmailandSMSConsole
{
    public class SmtpEmailService
    {
        private readonly EmailConfiguration _configuration;
        public SmtpEmailService() {
            _configuration = new EmailConfiguration()
            {
                From = "super.novakvova@ukr.net",
                SmtpServer = "smtp.ukr.net",
                Port = 2525,
                UserName = "super.novakvova@ukr.net",
                Password = "MVjWbajb9tGqiHgK"
            };
        }
        public void Send(Message message)
        {
            var body = new TextPart("html")
            {
                Text = message.Body,
                
            };
            //string path = @"C:\Users\novak\Desktop\images\generate.jpeg";
            // create an image attachment for the file located at path
            var attachment = new MimePart("image", "jpeg")
            {
               // Content = new MimeContent(File.OpenRead(path)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64
               // FileName = Path.GetFileName(path)
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);
            
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = multipart;

            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                    client.Authenticate(_configuration.UserName, _configuration.Password);
                    client.Send(emailMessage);
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine("Send message error {0}", ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public void DownloadMessages()
        {
            using (var client = new ImapClient())
            {
                client.Connect("imap.ukr.net", 993, SecureSocketOptions.SslOnConnect);
                client.Authenticate(_configuration.UserName, _configuration.Password);
                client.Inbox.Open(FolderAccess.ReadOnly);
                var uids = client.Inbox.Search(SearchQuery.All);
                foreach(var uid in uids)
                {
                    var message = client.Inbox.GetMessage(uid);
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("FROM: {0}",message.From);
                    Console.WriteLine("Subject: {0}", message.Subject);
                    message.WriteTo(string.Format("{0}.eml", uid));
                }
                client.Disconnect(true);
            }
        }
    }
}
