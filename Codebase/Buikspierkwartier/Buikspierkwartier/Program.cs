using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Buikspierkwartier;

namespace Landoefeningen
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static List<Oefening> Oefeningen = new List<Oefening>();

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            Program.LeesOefeningen(service);
            Programma p = Program.MaakTraining(service);
            //Console.WriteLine(p);
            p.ProgrammaToPdf();
            Console.Read();


        }

        public static void LeesOefeningen(SheetsService service)
        {
            // Define request parameters.
            String spreadsheetId = "1_sLp07EmQcQ3y35jD6Bg8hAmCnqS3fuSGItfF183hOw";
            String range = "Oefeningen!A2:I";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> rows = response.Values;
            if (rows != null && rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    //Console.WriteLine("Naam: \t\t{0}\nOmschrijving:\t{1}\nMakkelijk:\t{2}\nGemiddeld:\t{3}\nZwaar:\t\t{4}\n", row[0], row[1], row[2], row[3], row[4]);
                    Program.Oefeningen.Add(new Oefening((string)row[0], (string)row[1], (string)row[2], (string)row[3], (string)row[4], (string)row[5], (string)row[6], (string)row[7], (string)row[8]));
                }
            }
            else
            {
                Console.WriteLine("Geen oefeningen gevonden.");
            }
        }

        public static Programma MaakTraining(SheetsService service)
        {
            String spreadsheetId = "1_sLp07EmQcQ3y35jD6Bg8hAmCnqS3fuSGItfF183hOw";
            String range = "Schema!A2:B";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> rows = response.Values;

            Programma programma = new Programma();

            if (rows != null && rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    programma.AddOefening((string)row[0], (string)row[1]);
                }
            }
            else
            {
                Console.WriteLine("Geen programma gevonden.");
            }
            return programma;
        }
    }
}
