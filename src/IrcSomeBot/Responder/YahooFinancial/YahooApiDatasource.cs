using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder.YahooFinancial
{
    public class YahooApiDatasource : IStockTickerDataSource
    {
        public IEnumerable<string> GetPricingData(string ticker, DateTime requestTime)
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
                    yield return
                        string.Format("{0} ({1}) - {2:HH:mm:ss tt} - {3} ({4}) | Op: {5} | Lo-Hi {6}-{7}",
                            yahooFinanceApiData.Name,
                            ticker.ToUpper(),
                            requestTime,
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