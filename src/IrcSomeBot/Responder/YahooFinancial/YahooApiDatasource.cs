using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder.YahooFinancial
{
    public class YahooApiDatasource : IStockTickerDataSource
    {
        public IEnumerable<string> GetPricingData(string ticker)
        {
            var pricing = YahooFinanceApi.Request(ticker.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));

            foreach (var yahooFinanceApiData in pricing)
            {
                if (yahooFinanceApiData.Name.Equals("N/A", StringComparison.CurrentCultureIgnoreCase))
                {
                    yield return "Ticker not found!";
                }
                else
                {
                    var lastTradeDate = yahooFinanceApiData.LastTradeDate;
                    var lastTradeTime = yahooFinanceApiData.LastTradeTime;
                    DateTime lastTradeDateTime;
                    DateTime.TryParse(lastTradeDate + " " + lastTradeTime, out lastTradeDateTime);
                    yield return
                        string.Format("{0} ({1}) - {2:dddd @ h:mmtt} - {3} ({4}) | Op: {5} | Lo-Hi {6}-{7}",
                            yahooFinanceApiData.Name,
                            ticker.ToUpper(),
                            lastTradeDateTime,
                            yahooFinanceApiData.LastTradePrice,
                            yahooFinanceApiData.PercentChange,
                            yahooFinanceApiData.Open,
                            yahooFinanceApiData.DayLow,
                            yahooFinanceApiData.DayHigh
                            );
                }
            }
        }
    }
}