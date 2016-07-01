using System.Collections.Generic;
using System.IO;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;

namespace IrcSomeBot.Responder.YahooFinancial
{
    public static class YahooFinanceApi
    {
        private static string _apiUrl = "http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}";

        public static List<YahooFinanceApiData> Request(string[] tickers)
        {
            var tickerTransformed = string.Join("+", tickers);
            using (var client = new WebClient())
            {
                var result = client.DownloadString(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}", tickerTransformed, "snabopc1p2ghc8wvrl1"));
                return Parse(result);
            }

        }

        public static List<YahooFinanceApiData> Parse(string csvData)
        {
            var prices = new List<YahooFinanceApiData>();

            var csv = new CsvReader(new StringReader(csvData), new CsvConfiguration { HasHeaderRecord = false });
            while (csv.Read())
            {
                var p = new YahooFinanceApiData
                {
                    Symbol = csv.GetField(0),
                    Name = csv.GetField(1)
                };
                if (p.Name != "N/A")
                {
                    p.Bid = csv.GetField<decimal>(3);
                    p.Open = csv.GetField<decimal>(4);
                    p.PreviousClose = csv.GetField<decimal>(5);
                    p.Change = csv.GetField<decimal>(6);
                    p.PercentChange = csv.GetField(7); //.Replace("\"", string.Empty);
                    p.DayLow = csv.GetField<decimal>(8);
                    p.DayHigh = csv.GetField<decimal>(9);
                    p.AfterHoursChange = csv.GetField(10);
                    p.WeekRange52 = csv.GetField(11);
                    p.Volume = csv.GetField<decimal>(12);
                    p.PE = csv.GetField(13);
                    p.LastTradePrice = csv.GetField<decimal>(14);
                }
                prices.Add(p);
            }

            return prices;
        }
    }
}