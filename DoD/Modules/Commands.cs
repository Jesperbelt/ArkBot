using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoD;
using Table;

namespace Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        public enum guilds : ulong
        {
            DoD = 684822117618811140,
            SHR = 1024869376085737533,
            SAO = 772805666280308757,
            VAL = 900773269504942191,
            test = 643371773290610688,
            n420 = 741474736068100186,
            FTW = 677564139421302834,
            DOM = 730186186114465793,
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
        [Command("help")]
        public async Task Help()
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            await ReplyAsync($"This bot has the following commands:\n1. ``!add`` or ``!add @user``\n*Required command to add yourself or @user to the bank bot.*\n2. ``!info`` or ``!info @user``\n*Displays name,startdate and exemption from user that issued the command or user @*\n3. ``!total`` or ``!total @user``\n*Displays total banked in personal*\n4. ``!tracker`` or ``!tracker @user``\n*Displays total banked to guild*\n5. ``!rename name``\n*Rename yourself in the bank bot, BEWARE IF NOT ASKED BY BANKER YOU MAY NOT SEE TOTALS*\n6. ``!gear``\n*Calculate gear stats, issue the command to find how it works*");
        }
        [Command("info")]
        public async Task Info(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string name = (string)Context.User.Username;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            Console.WriteLine("Before Info");
            if (!(user == null))
            {
                userid = (long)user.Id;
                name = (string)user.Username;
            }
                Console.WriteLine("inside");
                List<Person_info> person_info = new List<Person_info>();
                List<Guild> guild_info = new List<Guild>();
                try
                {
                    person_info = dbmethod.SelectPersonID(userid, sguild);
                    guild_info = dbmethod.SelectStartdate(guildid,userid,sguild);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                if (!(person_info.Count > 0))
                {
                    await ReplyAsync($"{name} does not exist, please perform ``!add``.");
                }
                else
                {
                    await ReplyAsync($"Name: {person_info[0].name}\nStartdate: {guild_info[0].startdate}\nExemption: {guild_info[0].exemption}");
                }
        }
        [Command("total")]
        public async Task Total(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            if (!(user==null))
            {
                userid = (long)user.Id;
                Console.WriteLine(userid);
            }
                SheetQuerry sheetQuerry = new SheetQuerry();
                sheetQuerry.SelectSheet(sguild);
                List<Data_bank> rss = new List<Data_bank>();
                List<Person_info> person_info = new List<Person_info>();
                List<Table.Color> color = new List<Table.Color>();
                DbQuerry t = new DbQuerry();
            try
                {
                    rss = t.SelectAllRss("personal", userid, sguild);
                    person_info = dbmethod.SelectPersonID(userid,sguild);
                    color = t.SelectColor(guildid, sguild);
                }
                catch (Exception e)
                {

                }
                if (person_info.Count > 0)
                {
                    foreach (var row in rss)
                    {
                    Console.WriteLine("Inside resource count");
                        Ftotal += row.food;
                    Console.WriteLine("after count 1");
                    Ptotal += row.parts;
                        Etotal += row.electric;
                        Gtotal += row.gas;
                        Ctotal += row.cash;
                        Stotal += row.shadow;
                    }
                Console.WriteLine("before color set");
                Console.WriteLine(sguild);
                food = color[0].food;
                Console.WriteLine("after color set");
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
            Console.WriteLine($"{sguild}");
            if (!(user == null))
            {
                userid = (long)user.Id;
            }
                SheetQuerry sheetQuerry = new SheetQuerry();
                sheetQuerry.SelectSheet(sguild);
                List<Data_bank> rss = new List<Data_bank>();
                List<Table.Color> color = new List<Table.Color>();
                List<Person_info> person_info = new List<Person_info>();
                List<Guild> guild = new List<Guild>();
            double[] Multiplier = { 1, 1, 1, 1, 1 };
            if (sguild == "DOM")
            {
                Console.WriteLine("Set DOM multiplier");
                Multiplier[0] = 2;
                Multiplier[1] = 2;
                Multiplier[2] = 2;
                Multiplier[3] = 2;
                Multiplier[4] = 1;
            }
            if (sguild == "SHR")
            {
                Console.WriteLine("Set SHR multiplier");
                Multiplier[0] = 3;
                Multiplier[1] = 3;
                Multiplier[2] = 3;
                Multiplier[3] = 3;
                Multiplier[4] = 3;
            }
            try
                {
                    rss = dbmethod.SelectTrackerRss(sguild, "guild", userid);
                    color = dbmethod.SelectColor(guildid, sguild);
                    person_info = dbmethod.SelectPersonID(userid, sguild);
                    guild = dbmethod.SelectStartdate(guildid, userid, sguild);
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
                    Console.WriteLine($"row food: {row.food}");
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
                    int weeks = ((int)(Enddate - Startdate).TotalDays / 7);
                    weeks = weeks - guild[0].exemption;
                if(!(guild[0].startdate == null))
                {
                    if (Ftotal < (weeks*Multiplier[0]) || Ptotal < (weeks * Multiplier[1]) || Etotal < (weeks * Multiplier[2]) || Gtotal < (weeks * Multiplier[3]) || Ctotal < (weeks * Multiplier[4]))
                    {
                        string message2 = ($"\n{person_info[0].name} owes the Guild:\n");
                        if (Ftotal < (weeks * Multiplier[0]))
                        {
                            message2 += ($"<@&{food}>: {Math.Round((weeks * Multiplier[0]) - Ftotal, 2)}M\n");
                        }
                        if (Ptotal < (weeks * Multiplier[1]))
                        {
                            message2 += ($"<@&{parts}>: {Math.Round((weeks * Multiplier[1]) - Ptotal, 2)}M\n");
                        }
                        if (Etotal < (weeks * Multiplier[2]))
                        {
                            message2 += ($"<@&{electric}>: {Math.Round((weeks * Multiplier[2]) - Etotal, 2)}M\n");
                        }
                        if (Gtotal < (weeks * Multiplier[3]))
                        {
                            message2 += ($"<@&{gas}>: {Math.Round((weeks * Multiplier[3]) - Gtotal, 2)}M\n");
                        }
                        if (Ctotal < (weeks * Multiplier[4]))
                        {
                            message2 += ($"<@&{cash}>: {Math.Round((weeks * Multiplier[4]) - Ctotal, 2)}M\n");
                        }
                        await ReplyAsync(message1 + message2);
                    }
                    else
                    {
                        List<double> calc = new List<double>();
                        if (Ftotal < 1|| Ptotal < 1|| Etotal < 1|| Gtotal < 1|| Ctotal < 1)
                        {
                            calc.Add(0.0);
                        }
                        else
                        {
                            calc.Add(Ftotal / Multiplier[0]);
                            calc.Add(Ptotal / Multiplier[1]); 
                            calc.Add(Etotal / Multiplier[2]); 
                            calc.Add(Gtotal / Multiplier[3]); 
                            calc.Add(Ctotal / Multiplier[4]);
                        }
                        double min = calc.Min();
                        double credit = min - weeks;
                        await ReplyAsync(message1 + $"\nThank you, you have fullfilled the required amount :hugging:\n{person_info[0].name}, you got credit for **{Math.Round(credit, 0)}** weeks");
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
        [Command("totals")]
        public async Task Totals([Remainder] string remain = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);

            SheetQuerry sheetQuerry = new SheetQuerry();
            sheetQuerry.SelectSheet(sguild);
            List<Data_bank> rss = new List<Data_bank>();
            List<Person_info> person_info = new List<Person_info>();
            List<Table.Color> color = new List<Table.Color>();
            DbQuerry t = new DbQuerry();
            try
            {
                if (remain == null)
                {
                    await ReplyAsync($"Please provide entry Type. Like `Personal`");
                }
                else
                {
                    rss = t.SelectAllRssByType(remain, sguild);
                    person_info = dbmethod.SelectPersonID(userid, sguild);
                    color = t.SelectColor(guildid, sguild);
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
                    await ReplyAsync($"{sguild} has in {remain}:\n<@&{food}>: {Math.Round(Ftotal, 2)}M\n<@&{parts}>: {Math.Round(Ptotal, 2)}M\n<@&{electric}>: {Math.Round(Etotal, 2)}M\n<@&{gas}>: {Math.Round(Gtotal, 2)}M\n<@&{cash}>: {Math.Round(Ctotal, 2)}M\n<@&{shadow}>: {Stotal}");
                }
            }
            catch (Exception e)
            {
                await ReplyAsync($"A error occured");
            }

        }
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("flush")]
        public async Task Flush(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            dbmethod.DeleteBankData(sguild);
            await ReplyAsync("Tables correctly emptied");
        }

        [Command("add")]
        public async Task Add(IGuildUser user = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string name = (string)Context.User.Username;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            if (!(user == null))
            {
                userid = (long)user.Id;
                name = (string)user.Username;
            }
            Console.WriteLine("add");
                Console.WriteLine("inside");
                List<Person_info> person_info = new List<Person_info>();
                try
                {
                    person_info = dbmethod.SelectPersonID(userid, sguild);
                }
                catch (Exception e)
                {

                }
                if (!(person_info.Count > 0))
                {
                    dbmethod.InsertPerson(guildid, userid, name, sguild);
                    await ReplyAsync($"{name} added.");
                }
                else
                {
                    await ReplyAsync($"{person_info[0].name} Already exists!");
                }
            
        }
        //incomplete
        [Command("rename")]
        public async Task Rename([Remainder]string remain = null)
        {
            long userid = (long)Context.User.Id;
            long guildid = (long)Context.Guild.Id;
            string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
            string[] input = remain.Split(' ', 2);
            if (input.Length > 1)
            {
                Console.WriteLine($"Parse rename remain [0] : {userid}\n name: {input[1]}");
                //userid = Convert.ToInt64(input[0]);
                String regString = Regex.Match(input[0], @"\d+").Value;
                Console.WriteLine(regString);
                try
                {
                    userid = Convert.ToInt64(regString);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                String name = input[1];
                bool succes = dbmethod.UpdateName(sguild, userid, name);
                if (succes)
                {
                    await ReplyAsync($"Succesfully renamed you to: ``{name}``");
                }
                else
                {
                    await ReplyAsync($"Something went wrong.");
                }
            }
            if (input.Length == 1)
            {
                String name = input[0];
                bool succes = dbmethod.UpdateName(sguild, userid, name);
                if (succes)
                {
                    await ReplyAsync($"Succesfully renamed you to: ``{name}``");
                }
                else
                {
                    await ReplyAsync($"Something went wrong.");
                }
            }
            Console.WriteLine($"{input[0]} | {input[1]}"); 
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
                            await ReplyAsync($"The basestat is: {Math.Round(basestat,2)}");
                            Console.WriteLine($"{basestat}");
                        }
                        else if (gear.Length == 3 && Double.Parse(gear[0]) >= 0 && Double.Parse(gear[1]) >= 0 && Double.Parse(gear[2]) >= 0)
                        {
                            double basestat = Double.Parse(gear[0]) / (1 + (Double.Parse(gear[1]) / 10));
                            double upgradestat = ((basestat / 10) * Double.Parse(gear[2])) + basestat;
                            await ReplyAsync($"The upgradestat is: {Math.Round(upgradestat,2)}");
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
        [RequireUserPermission(GuildPermission.Administrator)]
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
                string sguild = (string)Enum.GetName(typeof(guilds), (ulong)guildid);
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
                            dbmethod.UpdateExemption(guildid, (long)user.Id, weeks, sguild);
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