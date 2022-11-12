using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Table;

namespace Context
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Person_info> person_info { get; set; }
        public DbSet<Data_bank> data_bank { get; set; }
        public DbSet<Color> color { get; set; }
        public DbSet<Guild> guild { get; set; }
        public static string dbname { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string mysql = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
            mysql = mysql.Replace("Guild",$"{dbname}");
            optionsBuilder.UseMySQL(mysql);
        }
    }
}
