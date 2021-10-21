using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoD
{
    public class DoD_Context : DbContext
    {
        public DbSet<Person_info> person_info { get; set; }
        public DbSet<Data_bank> data_bank { get; set; }
        public DbSet<Color> color { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.99;database=bot_test;user=DoD;password=%%Truckers**;SSL Mode=None");
        }
    }
}
