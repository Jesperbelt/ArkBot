using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules;

namespace DoD
{
    public class DbQuerry
    {
        public List<Data_bank> SelectAllRss(string type, long userid,string guild)
        {
            List<Data_bank> rss = new List<Data_bank>();
                var context = new DbContext();
            DbContext.dbname = guild;
                rss = context.data_bank.AsQueryable()
                .Where(d => d.type.Equals(type) && d.id == userid).ToList();
            Console.WriteLine("inside selectrss");
            return rss;
        }
        public List<Data_bank> SelectTrackerRss(string guild, string type, long userid)
        {
            List<Data_bank> rss = new List<Data_bank>();
            var context = new DbContext();
            DbContext.dbname = guild;
            rss = context.data_bank.AsQueryable()
            .Where(d => d.type.Equals(type) && d.id == userid && d.guild == guild).ToList();
            Console.WriteLine("inside selectrss");
            return rss;
        }
        public int SelectLineId(string guild)
        {
            int maxnumb = 0;
            var context = new DbContext();
            DbContext.dbname = guild;
                try
                {
                    maxnumb = context.data_bank.AsQueryable()
                .Max(d => d.lineid);
                    Console.WriteLine(maxnumb);
                }
                catch (Exception e)
                {
                    maxnumb = 0;
                }
            return maxnumb;
        }
        public List<Color> SelectColor(long id,string guild)
        {
            List<Color> color = new List<Color>();
                var context = new DbContext();
            DbContext.dbname = guild;
                color = context.color.AsQueryable()
                .Where(row => row.guild == id)
                .ToList();
            return color;
        }
        public List<Person_info> SelectPersonID(long userid,string guild)
        {
            List<Person_info> person = new List<Person_info>();
            var context = new DbContext();
            DbContext.dbname = guild;
            person = context.person_info.AsQueryable()
            .Where(row => row.id == userid)
            .ToList();
            return person;
        }

        public List<Person_info> SelectPersonName(string guild, string uname)
        {
            List<Person_info> person = new List<Person_info>();
            DbContext.dbname = guild;
            var context = new DbContext();
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
            if (person == null)
            {
                List<Person_info> nan = new List<Person_info>();
                nan[0].id = 0;
                return nan;
            }
            return person;
        }
        public void InsertPerson(long guildid, long userid, string name,string guild)
        {

            var context = new DbContext();
            DbContext.dbname = guild;
            DateTime date = DateTime.Now;
            var std = new Person_info()
            {
                id = userid,
                name = name,
            };
            context.person_info.Add(std);
            var std1 = new Guild()
            {
                guild = guildid,
                discordid = userid,
                startdate = DateTime.Now.ToString(),
                exemption = 0,
            };
            context.guild.Add(std1);
            context.SaveChanges();
        }
        public void UpdateExemption(long guildid, long userid,int weeks,string guild)
        {
            Person_info p = new Person_info();
            var context = new DbContext();
            DbContext.dbname = guild;
            List<Guild> p2 = new List<Guild>();
            try
            {
                p2 = context.guild.AsQueryable()
                .Where(row => row.discordid == userid && row.guild == guildid).ToList();
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
        public List<Guild> SelectStartdate(long guildid,long userid,string guild)
        {
            Console.WriteLine("inside SelectStartdate");
            List<Guild> guildlist = new List<Guild>();
                var context = new DbContext();
            DbContext.dbname=guild;
                guildlist = context.guild.AsQueryable()
                .Where(row => row.discordid == userid && row.guild == guildid)
                .ToList();
            return guildlist;
        }

        public void DeleteBankData(string guild)
        {
            List<Data_bank> rss = new List<Data_bank>();
            var context = new DbContext();
            DbContext.dbname = guild;
            rss = context.data_bank.AsQueryable()
            .Where(d => d.lineid>0).ToList();
            foreach(var row in rss)
            {
                context.data_bank.Remove(row);
            }
            context.SaveChanges();
        }

        public bool UpdateName(string guild,long userid,string name)
        {
            List<Person_info> person = new List<Person_info>();
            var context = new DbContext();
            DbContext.dbname = guild;
            person = context.person_info.AsQueryable()
            .Where(row => row.id == userid)
            .ToList();
            person[0].name = name;
            try
            {
                context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}