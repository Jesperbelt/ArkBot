using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoD;

namespace Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        public enum guilds : ulong
        {
            DoD = 684822117618811140,
            SHR = 696820705651720342,
            SAO = 772805666280308757
        };
        long food;
        long parts;
        long electric;
        long gas;
        long cash;
        long shadow;
        int Ftotal = 0;
        int Ptotal = 0;
        int Etotal = 0;
        int Gtotal = 0;
        int Ctotal = 0;
        int Stotal = 0;
        DbQuerry dbmethod = new DbQuerry();
        [Command("total")]
        public async Task Total(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            if (!(user==null))
            {
                userid = (long)user.Id;
            }
            if (Context.Guild.Id == (ulong)guilds.DoD)
            {
                List<Data_bank> rss = new List<Data_bank>();
                List<DoD.Color> color = new List<DoD.Color>();
                DbQuerry t = new DbQuerry();
                rss = t.SelectRss("DoD","personal", userid);
                color = t.SelectColor("DoD");
                foreach (var row in rss)
                {
                    Ftotal += row.food;
                    Ptotal += row.parts;
                    Etotal += row.electric;
                    Gtotal += row.gas;
                    Ctotal += row.cash;
                    Stotal += row.shadow;
                }
                food = color[0].food;
                parts = color[0].parts;
                electric = color[0].electric;
                gas = color[0].gas;
                cash = color[0].cash;
                shadow = color[0].shadow;
                await ReplyAsync($"{rss[0].name} has in Personal:\n<@&{food}>: {Ftotal}M\n<@&{parts}>: {Ptotal}M\n<@&{electric}>: {Etotal}M\n<@&{gas}>: {Gtotal}M\n<@&{cash}>: {Ctotal}M\n<@&{shadow}>: {Stotal}");
            }
        }
        [Command("tracker")]
        public async Task Tracker(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            if (!(user == null))
            {
                userid = (long)user.Id;
            }
            if (Context.Guild.Id == (ulong)guilds.DoD)
            {
                List<Data_bank> rss = new List<Data_bank>();
                List<DoD.Color> color = new List<DoD.Color>();
                List<Person_info> person_info = new List<Person_info>();
                rss = dbmethod.SelectRss("DoD", "guild", userid);
                color = dbmethod.SelectColor("DoD");
                person_info = dbmethod.SelectPerson("DoD", userid);
                foreach (var row in rss)
                {
                    Ftotal += row.food;
                    Ptotal += row.parts;
                    Etotal += row.electric;
                    Gtotal += row.gas;
                    Ctotal += row.cash;
                    Stotal += row.shadow;
                }
                food = color[0].food;
                parts = color[0].parts;
                electric = color[0].electric;
                gas = color[0].gas;
                cash = color[0].cash;
                shadow = color[0].shadow;
                string message1=($"{rss[0].name} has in Guild:\n<@&{food}>: {Ftotal}M\n<@&{parts}>: {Ptotal}M\n<@&{electric}>: {Etotal}M\n<@&{gas}>: {Gtotal}M\n<@&{cash}>: {Ctotal}M\n<@&{shadow}>: {Stotal}");
                DateTime Enddate = DateTime.Now;
                DateTime Startdate = Convert.ToDateTime(person_info[0].startdate);
                int days = ((int)(Enddate - Startdate).TotalDays/7);
                if (Ftotal<days || Ptotal < days|| Etotal < days|| Gtotal < days|| Ctotal < days)
                {
                    string message2 = ($"\n{rss[0].name} owes the Guild:\n");
                    if (Ftotal < days)
                    {
                        message2 += ($"<@&{food}>: {days - Ftotal}\n");
                    }
                    if (Ptotal < days)
                    {
                        message2 += ($"<@&{parts}>: {days - Ptotal}\n");
                    }
                    if (Etotal < days)
                    {
                        message2 += ($"<@&{electric}>: {days - Etotal }\n");
                    }
                    if (Gtotal < days)
                    {
                        message2 += ($"<@&{gas}>: {days - Gtotal }\n");
                    }
                    if (Ctotal < days)
                    {
                        message2 += ($"<@&{cash}>: {days - Ctotal}\n");
                    }
                    await ReplyAsync(message1+message2);
                }
                else
                {
                    await ReplyAsync(message1+"\nThank you, you have fullfilled the required amount :hugging:");
                }
            }
        }
        [Command("add")]
        public async Task Add(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            string name = (string)Context.User.Username;
            if (!(user == null))
            {
                userid = (long)user.Id;
                name = (string)user.Username;
            }
            if (Context.Guild.Id == (ulong)guilds.DoD)
            {
                List<Person_info> person_info = new List<Person_info>();
                person_info = dbmethod.SelectPerson("DoD", userid);
                if (!(person_info[0].id > 0))
                {
                    dbmethod.InsertPerson("DoD", userid, name);
                    await ReplyAsync($"{name} added.");
                }
                else
                {
                    await ReplyAsync($"{person_info[0].name} Already exists!");
                }
            }
        }
        [Command("rename")]
        public async Task Rename([Remainder]string remain = null)
        {
            long userid = (long)Context.User.Id;
            string name = "";
            if (!(remain == null))
            {
                name = remain;
            }
            Console.WriteLine($"{name}"); 
        }
    }
}