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
            SAO = 772805666280308757,
            test = 643371773290610688,
            n420 = 741474736068100186
        };
        long food;
        long parts;
        long electric;
        long gas;
        long cash;
        long shadow;
        double Ftotal = 0;
        double Ptotal = 0;
        double Etotal = 0;
        double Gtotal = 0;
        double Ctotal = 0;
        double Stotal = 0;
        DbQuerry dbmethod = new DbQuerry();
        [Command("total")]
        public async Task Total(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            if (!(user==null))
            {
                userid = (long)user.Id;
            }

                SheetQuerry sheetQuerry = new SheetQuerry();
                sheetQuerry.SelectSheet();
                List<Data_bank> rss = new List<Data_bank>();
                List<Person_info> person_info = new List<Person_info>();
                List<DoD.Color> color = new List<DoD.Color>();
                DbQuerry t = new DbQuerry();
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            try
                {
                    rss = t.SelectAllRss("personal", userid);
                    person_info = dbmethod.SelectPersonID("DoD", userid);
                    color = t.SelectColor(guildid);
                }
                catch (Exception e)
                {

                }
                if (person_info.Count > 0)
                {
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
                    await ReplyAsync($"{person_info[0].name} has in Personal:\n<@&{food}>: {Math.Round(Ftotal, 2)}M\n<@&{parts}>: {Math.Round(Ptotal, 2)}M\n<@&{electric}>: {Math.Round(Etotal, 2)}M\n<@&{gas}>: {Math.Round(Gtotal, 2)}M\n<@&{cash}>: {Math.Round(Ctotal, 2)}M\n<@&{shadow}>: {Stotal}");
                }
                else
                {
                    await ReplyAsync($"You dont exist please perform `!add`");
                }
        }
        [Command("tracker")]
        public async Task Tracker(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds),(ulong)guildid);
            Console.WriteLine(sguild);
            if (!(user == null))
            {
                userid = (long)user.Id;
            }
                SheetQuerry sheetQuerry = new SheetQuerry();
                sheetQuerry.SelectSheet();
                List<Data_bank> rss = new List<Data_bank>();
                List<DoD.Color> color = new List<DoD.Color>();
                List<Person_info> person_info = new List<Person_info>();
                List<Guild> guild = new List<Guild>();
            try
                {
                    rss = dbmethod.SelectTrackerRss(sguild, "guild", userid);
                    color = dbmethod.SelectColor(guildid);
                    person_info = dbmethod.SelectPersonID("DoD", userid);
                    guild = dbmethod.SelectStartdate(guildid, userid);
                foreach(var row in guild)
                    {
                    Console.WriteLine($"{guild[0].startdate}");
                    }
                }
                catch (Exception e)
                {

                }
                if (person_info.Count > 0)
                {
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
                    string message1 = ($"{person_info[0].name} has in Guild:\n<@&{food}>: {Math.Round(Ftotal, 2)}M\n<@&{parts}>: {Math.Round(Ptotal, 2)}M\n<@&{electric}>: {Math.Round(Etotal, 2)}M\n<@&{gas}>: {Math.Round(Gtotal, 2)}M\n<@&{cash}>: {Math.Round(Ctotal, 2)}M\n<@&{shadow}>: {Stotal}");
                    DateTime Enddate = DateTime.Now;
                    DateTime Startdate = Convert.ToDateTime(guild[0].startdate);
                    int days = ((int)(Enddate - Startdate).TotalDays / 7);
                    days = days - guild[0].exemption;
                if(!(guild[0].startdate == null))
                {
                    if (Ftotal < days || Ptotal < days || Etotal < days || Gtotal < days || Ctotal < days)
                    {
                        string message2 = ($"\n{person_info[0].name} owes the Guild:\n");
                        if (Ftotal < days)
                        {
                            message2 += ($"<@&{food}>: {Math.Round(days - Ftotal, 2)}M\n");
                        }
                        if (Ptotal < days)
                        {
                            message2 += ($"<@&{parts}>: {Math.Round(days - Ptotal, 2)}M\n");
                        }
                        if (Etotal < days)
                        {
                            message2 += ($"<@&{electric}>: {Math.Round(days - Etotal, 2)}M\n");
                        }
                        if (Gtotal < days)
                        {
                            message2 += ($"<@&{gas}>: {Math.Round(days - Gtotal, 2)}M\n");
                        }
                        if (Ctotal < days)
                        {
                            message2 += ($"<@&{cash}>: {Math.Round(days - Ctotal, 2)}M\n");
                        }
                        await ReplyAsync(message1 + message2);
                    }
                    else
                    {
                        await ReplyAsync(message1 + "\nThank you, you have fullfilled the required amount :hugging:");
                    }
                }
                else
                {
                    await ReplyAsync(message1 + "\nNo ``startdate`` set:exclamation:");
                }
                }
                else
                {
                    await ReplyAsync($"You dont exist please perform `!add`");
                }
                
            
        }
        [Command("add")]
        public async Task Add(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string name = (string)Context.User.Username;
            if (!(user == null))
            {
                userid = (long)user.Id;
                name = (string)user.Username;
            }
            Console.WriteLine("add");
            if (Context.Guild.Id == (ulong)guilds.DoD || Context.Guild.Id == (ulong)guilds.n420)
            {
                Console.WriteLine("inside");
                List<Person_info> person_info = new List<Person_info>();
                try
                {
                    person_info = dbmethod.SelectPersonID("DoD", userid);
                }
                catch (Exception e)
                {

                }
                if (!(person_info.Count > 0))
                {
                    dbmethod.InsertPerson(guildid, userid, name);
                    await ReplyAsync($"{name} added.");
                }
                else
                {
                    await ReplyAsync($"{person_info[0].name} Already exists!");
                }
            }
        }
        //incomplete
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
        [Command("gear")]
        public async Task gear([Remainder] string remain = null)
        {
            if (remain == ""||remain ==null)
            {
                await ReplyAsync("You didn't provide the required parameters\nUse command like this:\n``!gear 99 10`` < this will show the base stat\n``!gear 99 10 30`` < this will show upgraded stats");
            }
            else
            {
                string[] gear = remain.Split();
                if (gear.Length == 1)
                {
                    await ReplyAsync("You didn't provide the required parameters\nUse command like this:\n``!gear 99 10`` < this will show the base stat\n``!gear 99 10 30`` < this will show upgraded stats");
                }
                else
                {
                    try
                    {
                        if (gear.Length == 2 && Double.Parse(gear[0]) >= 0 && Double.Parse(gear[1]) >= 0)
                        {
                            double basestat = Double.Parse(gear[0]) / (1 + (Double.Parse(gear[1]) / 10));
                            await ReplyAsync($"The basestat is: {basestat}");
                            Console.WriteLine($"{basestat}");
                        }
                        else if (gear.Length == 3 && Double.Parse(gear[0]) >= 0 && Double.Parse(gear[1]) >= 0 && Double.Parse(gear[2]) >= 0)
                        {
                            double basestat = Double.Parse(gear[0]) / (1 + (Double.Parse(gear[1]) / 10));
                            double upgradestat = ((basestat / 10) * Double.Parse(gear[2])) + basestat;
                            await ReplyAsync($"The upgradestat is: {upgradestat}");
                            Console.WriteLine($"{upgradestat}");
                        }
                        else
                        {
                            await ReplyAsync("Please don't use negative numbers!");
                        }

                    }
                    catch (Exception e)
                    {
                        await ReplyAsync("Please only use numbers!");
                    }
                }
            }
        }
        [RequireUserPermission(ChannelPermission.SendTTSMessages)]
        [Command("exempt")]
        public async Task Exempt([Remainder]string s =null)
        {
            if(true)
            {
                await ReplyAsync($"Failed to exempt");
            }
            else
            {
                var users = Context.Message.MentionedUsers;
                long guildid = (long)Context.Guild.Id;
                int count = 0;
                string usernames = "";
                int weeks = 0;
                string[] sa = s.Split();
                foreach (string o in sa)
                {
                    if (Int32.TryParse(o, out int i))
                    {
                        weeks = Int32.Parse(o);
                    }
                }
                if (users.Count > 0)
                {

                    foreach (var user in users)
                    {
                        Console.WriteLine($"{user.Id}");
                        try
                        {
                            dbmethod.UpdateExemption(guildid, (long)user.Id, weeks);
                            usernames = usernames + $"{user.Username},";
                        }
                        catch (Exception e)
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        await ReplyAsync($"Failed to exempt: {count}");
                    }
                    else
                    {
                        await ReplyAsync($"Succesfully exempted for {weeks} Users: {usernames}");
                    }
                }
                else
                {
                    await ReplyAsync($"Please mention the user you want to exempt.");
                }
            }
        }
    }
}