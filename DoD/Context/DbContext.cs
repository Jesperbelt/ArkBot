using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoD
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
            string mysql = System.IO.File.ReadAllText(@"C:\db.txt");
            mysql = mysql.Replace("Guild",$"{dbname}");
            optionsBuilder.UseMySQL(mysql);
        }
    }
}
