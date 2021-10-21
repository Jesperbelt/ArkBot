using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DoD
{
    public class SheetQuerry
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName ="DoDSHeetbot";
        static readonly string SpreadsheetId = "1XxR4kmRh2vy4Sg_d4HvrcZQwtAkCCEPUTTAoOXBIlyE";
        static readonly string sheet = "BankData";
        static SheetsService service;

        public void Sheet()
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
            ReadEntries();
        }
        void ReadEntries()
        {
            var range = $"{sheet}!B2:B2";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach(var row in values)
                {
                    Console.WriteLine(row[0]);
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }
        }
    }
}