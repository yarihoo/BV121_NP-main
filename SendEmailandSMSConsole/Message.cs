using System;
using System.Collections.Generic;
using System.Text;

namespace SendEmailandSMSConsole
{
    public class Message
    {
        /// <summary>
        /// Тема листа
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Тіло листа
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Кому адресоване пісьмо
        /// </summary>
        public string To { get; set; }
    }
}
