using HttpProtocolConsole.dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace HttpProtocolConsole
{
    public class Program
    {
        static string url = "https://bv012.novakvova.com";
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            var user = new RegisterUserDTO();
            user.FirstName = "Іван";
            user.SecondName = "Шкварочка";
            user.Email = "shkvarka@gmail.com";
            user.Phone = "+38(098)234-12-12";
            user.Password = "123456";
            user.ConfirmPassword = user.Password;
            string img = "C:\\Users\\novak\\Desktop\\images\\istockphoto-1369508766-170667a.jpg";
            user.Photo = ImageToBase64(img);
            RegisterUser(user);
            ReadDataServer();
        }

        private static string ImageToBase64(string path)
        {
            using(System.Drawing.Image img = System.Drawing.Image.FromFile(path))
            {
                using(MemoryStream m = new MemoryStream())
                {
                    img.Save(m, img.RawFormat);
                    byte[] bytes = m.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
        }
        private static void ReadDataServer()
        {
            WebRequest request = WebRequest.Create($"{url}/api/account/users");
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                var response = request.GetResponse();
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    //Console.WriteLine("Responce = {0}", data);

                    var users = JsonConvert.DeserializeObject<List<UserItemDTO>>(data);
                    foreach (var user in users)
                    {
                        Console.WriteLine("User: {0}", user.FirstName + " " + user.SecondName);
                        Console.WriteLine("Image: {0}{1}", url, user.Photo);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Error {0}", ex.Message); }
        }
    
        private static void RegisterUser(RegisterUserDTO registerUser)
        {
            string json = JsonConvert.SerializeObject(registerUser);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            WebRequest request = WebRequest.Create($"{url}/api/account/register");
            request.Method = "POST";
            request.ContentType = "application/json";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            try
            {
                var response = request.GetResponse();
                using(var stream = new StreamReader(response.GetResponseStream()))
                {
                    string data = stream.ReadToEnd();
                    Console.WriteLine(data);
                }
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
