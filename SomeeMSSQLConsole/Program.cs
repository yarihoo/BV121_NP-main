using SomeeMSSQLConsole.Data;
using SomeeMSSQLConsole.Data.Entities;
using System;

namespace SomeeMSSQLConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Робота з БД");
            //UserEntity user = new UserEntity
            //{
            //    FirstName = "Іван",
            //    SecondName = "Мельник",
            //    Phone = "98-45-23",
            //    Email = "ivan@gamil.com"
            //};
            //AppEFContext context = new AppEFContext();
            //context.Users.Add(user);
            //context.SaveChanges();
            AppEFContext context = new AppEFContext();
            foreach(var user in context.Users)
            {
                Console.WriteLine(user.SecondName+" "+user.FirstName);
            }
            
        }
    }
}
