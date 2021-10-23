using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;

namespace DoD
{
    public class DbQuerry
    {
        public List<Data_bank> SelectRss(string guild,string type,long userid)
        {
            List<Data_bank> rss = new List<Data_bank>();
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                rss = context.data_bank.AsQueryable()
                .Where(d => d.type.Equals(type) && d.id == userid).ToList();
            }
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
        public List<Color> SelectColor(string guild)
        {
            List<Color> color = new List<Color>();
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                color = context.color.AsQueryable()
                .Where(row => row.food > 0)
                .ToList();
            }
            return color;
        }
        public List<Person_info> SelectPersonID(string guild,long userid)
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
        public void InsertPerson(string guild, long userid,string name)
        {
            if (guild == "DoD")
            {
                var context = new DoD_Context();
                DateTime date = DateTime.Now;
                var std = new Person_info()
                {
                    id = userid,
                    name = name,
                    startdate = DateTime.Now.ToString(),
                };
                context.person_info.Add(std);
                context.SaveChanges();
            }
        }
    }
}