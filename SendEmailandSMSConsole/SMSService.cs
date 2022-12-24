using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SendEmailandSMSConsole
{
    public class SMSService
    {
        public void Send(string phone, string text)
        {
            string apiKey = "ua12b67a6e8a4d3c4408a6989edc4c01a616c23f12e071cf5a9f86c20463b906183489";
            string url = $"https://api.mobizon.ua/service/message/sendsmsmessage?recipient={phone}&text={text}&apiKey={apiKey}";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response= request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            using(Stream dataStream = response.GetResponseStream())
            {
                StreamReader sr = new StreamReader(dataStream);
                string data = sr.ReadToEnd();
                Console.WriteLine(data);
            }
            response.Close();
        }
    }
}
