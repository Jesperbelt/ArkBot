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

namespace DoD
{
    public class SheetQuerry
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName ="DoDSHeetbot";
        static readonly string SpreadsheetId = "1XxR4kmRh2vy4Sg_d4HvrcZQwtAkCCEPUTTAoOXBIlyE";
        static readonly string sheet = "BankData";
        static SheetsService service;
        DbQuerry dbmethod = new DbQuerry();
        static DateTime then;
        public void SelectSheet()
        {
            GoogleCredential credential;
            using(var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            updateDB();
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
        void updateDB()
        {
            int max = dbmethod.SelectLineId("DoD");
            if (max > 2) { max = max + 2; };
            var range = $"{sheet}!A{max}:K5757";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach(var row in values)
                {
                    Console.WriteLine($"{row[0]} | {row[1]} | {row[2]} | {row[3]} | {row[4]} | {row[5]} | {row[6]} | {row[7]} | {row[8]} | {row[9]} | {row[10]}");
                    List<Person_info> temp = new List<Person_info>();
                    temp = dbmethod.SelectPersonName("DoD", (string)row[1]);
                    long? id = null;
                    if (temp.Count>0)
                    {
                        id = temp[0].id;
                    }
                    var context = new DoD_Context();
                    var std = new Data_bank()
                    {
                        id = id,
                        lineid = Int32.Parse((string)row[0]),
                        name = (string)row[1],
                        date = (string)row[2],
                        guild = (string)row[3],
                        type = (string)row[4],
                        food = Double.Parse((string)row[5]),
                        parts = Double.Parse((string)row[6]),
                        electric = Double.Parse((string)row[7]),
                        gas = Double.Parse((string)row[8]),
                        cash = Double.Parse((string)row[9]),
                        shadow = Double.Parse((string)row[10]),
                    };
                    context.data_bank.Add(std);
                    context.SaveChanges();
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