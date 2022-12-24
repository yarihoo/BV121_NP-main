using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChatClientWPF
{
    public class ChatMessage
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public byte[] Serialize()
        {
            using (var m = new MemoryStream())
            {
                using(BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(UserId);
                    writer.Write(UserName);
                    writer.Write(Text);
                }
                return m.ToArray();
            }
        }
        public static ChatMessage Dessialize(byte [] data)
        {
            ChatMessage message = new ChatMessage();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    message.UserId = reader.ReadString();
                    message.UserName = reader.ReadString();
                    message.Text = reader.ReadString();
                }
            }
            return message;
        }
    }
}
