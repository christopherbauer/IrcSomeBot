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
                var result = client.DownloadString(string.Format("http://finance.yahoo.com/d/quotes.csv?s={0}&f={1}", tickerTransformed, "snabopc1p2ghc8wvrl1t1d1"));
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
                    Name = csv.GetField(1),
                    Bid = csv.GetField(3),
                    Open = csv.GetField(4),
                    PreviousClose = csv.GetField(5),
                    Change = csv.GetField(6),
                    PercentChange = csv.GetField(7),
                    DayLow = csv.GetField(8),
                    DayHigh = csv.GetField(9),
                    AfterHoursChange = csv.GetField(10),
                    WeekRange52 = csv.GetField(11),
                    Volume = csv.GetField(12),
                    PE = csv.GetField(13),
                    LastTradePrice = csv.GetField(14),
                    LastTradeTime = csv.GetField(15),
                    LastTradeDate = csv.GetField(16)
                };
                //.Replace("\"", string.Empty);
                prices.Add(p);
            }

            return prices;
        }
    }
}