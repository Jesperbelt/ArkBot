﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;

namespace DoD
{
    public class DbQuerry
    {
        public List<Data_bank> SelectAllRss(string type, long userid)
        {
            List<Data_bank> rss = new List<Data_bank>();
                var context = new DoD_Context();
                rss = context.data_bank.AsQueryable()
                .Where(d => d.type.Equals(type) && d.id == userid).ToList();
            Console.WriteLine("inside selectrss");
            return rss;
        }
        public List<Data_bank> SelectTrackerRss(string guild, string type, long userid)
        {
            List<Data_bank> rss = new List<Data_bank>();
            var context = new DoD_Context();
            rss = context.data_bank.AsQueryable()
            .Where(d => d.type.Equals(type) && d.id == userid && d.guild == guild).ToList();
            Console.WriteLine("inside selectrss");
            return rss;
        }
        public int SelectLineId(string guild)
        {
            int maxnumb = 0;
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                try
                {
                    maxnumb = context.data_bank.AsQueryable()
                .Max(d => d.lineid);
                    Console.WriteLine(maxnumb);
                }
                catch (Exception e)
                {
                    maxnumb = 2;
                }
            }
            return maxnumb;
        }
        public List<Color> SelectColor(long id)
        {
            List<Color> color = new List<Color>();
                var context = new DoD_Context();
                color = context.color.AsQueryable()
                .Where(row => row.guild == id)
                .ToList();
            return color;
        }
        public List<Person_info> SelectPersonID(string guild, long userid)
        {
            List<Person_info> person = new List<Person_info>();
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                person = context.person_info.AsQueryable()
                .Where(row => row.id == userid)
                .ToList();
            }
            return person;
        }

        public List<Person_info> SelectPersonName(string guild, string uname)
        {
            List<Person_info> person = new List<Person_info>();
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                try
                {
                    person = context.person_info.AsQueryable()
                    .Where(row => row.name == uname)
                    .ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            if (person == null)
            {
                List<Person_info> nan = new List<Person_info>();
                nan[0].id = 0;
                return nan;
            }
            return person;
        }
        public void InsertPerson(long guild, long userid, string name)
        {

            var context = new DoD_Context();
            DateTime date = DateTime.Now;
            var std = new Person_info()
            {
                id = userid,
                name = name,
            };
            context.person_info.Add(std);
            var std1 = new Guild()
            {
                guild = guild,
                discordid = userid,
                startdate = DateTime.Now.ToString(),
                exemption = 0,
            };
            context.guild.Add(std1);
            context.SaveChanges();
        }
        public void UpdateExemption(long guild, long userid,int weeks)
        {
            Person_info p = new Person_info();
            var context = new DoD_Context();
            List<Guild> p2 = new List<Guild>();
            try
            {
                p2 = context.guild.AsQueryable()
                .Where(row => row.discordid == userid && row.guild == guild).ToList();
                Console.WriteLine(p2[0].exemption);
                p2[0].exemption += weeks;
                Console.WriteLine(p2[0].exemption);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine(p.name);
        }
        public List<Guild> SelectStartdate(long guild,long userid)
        {
            List<Guild> guildlist = new List<Guild>();
                var context = new DoD_Context();
                guildlist = context.guild.AsQueryable()
                .Where(row => row.discordid == userid && row.guild == guild)
                .ToList();
            return guildlist;
        }

    }
}