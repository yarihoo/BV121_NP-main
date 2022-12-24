using FTPAccessSite.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FTPAccessSite.Data
{
    public class AppEFContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=vovawer.mssql.somee.com;Database=vovawer;User Id=xasew_SQLLogin_1;Password=c5qjhddu4o;");
        }
    }
}
