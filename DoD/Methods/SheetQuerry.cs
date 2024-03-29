using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DoD;
using Google.Apis.Sheets.v4.Data;
using Table;
using Context;
using System.Net;

namespace DoD
{
    public class SheetQuerry
    {
        public Dictionary<string, string> sheetid = new Dictionary<string, string>()
        {
            {"n420", "1EyqPNd0z8sAogR6NxlsyuTGpdTyBwRV3Ti1DJecd1kI"},
            {"VAL", "1dpBYsLLBTaOY5XPTQIdUwvauTwB8h0rNGRIWkXdc_HU"},
            {"SAO", "1n8YMi0_sb1hXphbl1wgO9uNvyHnIVLbWPSBDEJk6GAQ"},
            {"FTW", "1XxR4kmRh2vy4Sg_d4HvrcZQwtAkCCEPUTTAoOXBIlyE"},
            {"SHR", "1rodipreQnXxR76qDeg9LzKb_SNp5aFllmukQF7-sbfk"},
            {"DOM", "1nGxsE2XEZKjRV4voIHNpIIccEjw1yUZOS1UsQuXnQRY"},
            {"SVW","1Zkl63cdbeSXT3yVPdoiFhhn45F6JgMORfE--_jQP5Mc"},
            {"SWO","19klBz_vm15Yqf7JWnPwMceEoEI1dzBSaZUmZ-Xg5N8A"},
            {"n1662","1J_VP7FzjGkT42ijE6bQHzOU-WsE2I1FIAvFnOeCR49o"}
        };
        public static string SpreadsheetId { get; set; }
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "DoDSHeetbot";
        static readonly string sheet = "BankData";
        static SheetsService service;
        DbQuerry dbmethod = new DbQuerry();
        static DateTime then;
        public void SelectSheet(string guild)
        {
            SpreadsheetId = sheetid[$"{guild}"];
            GoogleCredential credential;
            using (var stream = new FileStream("/source/sheet/credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            updateDB(guild);
        }

        public void UpdateSheet()
        {
            var range = $"test!A1";
            var valueRange = new ValueRange();

            var objectlist = new List<object>() { "Updated" };
            valueRange.Values = new List<IList<object>> { objectlist };

            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();
        }
        void updateDB(string guild)
        {
            int max = dbmethod.SelectLineId(guild);
            max=max+2;
            Console.WriteLine(max);
                var range = $"{sheet}!A{max}:J5757";
                var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
                var response = request.Execute();
                var values = response.Values;
                if (values != null && values.Count > 0)
                {
                    int count = 1;
                    foreach (var row in values)
                    {
                        Console.WriteLine($"{row[0]} | {row[1]} | {row[2]} | {row[3]} | {row[4]} | {row[5]} | {row[6]} | {row[7]} | {row[8]} | {row[9]}");
                        List<Person_info> temp = new List<Person_info>();
                        int lineNr=0;
                        lineNr=Int32.Parse((string)row[0]);
                        temp = dbmethod.SelectPersonName(guild, (string)row[1]);
                        long? id = null;
                        if (temp.Count > 0)
                        {
                            id = temp[0].id;
                        }
                        var context = new DbContext();
                        DbContext.dbname = guild;
                            var std = new Data_bank()
                            {
                                id = id,
                                lineid = lineNr,
                                name = (string)row[1],
                                date = (string)row[2],
                                type = (string)row[3],
                                food = Double.Parse((string)row[4]),
                                parts = Double.Parse((string)row[5]),
                                electric = Double.Parse((string)row[6]),
                                gas = Double.Parse((string)row[7]),
                                cash = Double.Parse((string)row[8]),
                                shadow = Double.Parse((string)row[9]),
                            };
                            context.data_bank.Add(std);
                            context.SaveChanges();
                        
                        count = count + 1;
                    }
                    then = DateTime.Now.AddHours(1);
                }
                else
                {
                    Console.WriteLine("No data found");
                }
        }
    }
}