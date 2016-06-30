using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public class TickerResponder : IResponder
    {
        private readonly Dictionary<string, DateTime> _tickerTracker;
        private readonly TimeSpan _norepeat;

        public TickerResponder(TimeSpan norepeat)
        {
            _norepeat = norepeat;
            _tickerTracker = new Dictionary<string, DateTime>();
        }

        public bool HasResponse(string inputLine)
        {
            return inputLine.StartsWith(",");
        }

        public List<string> GetResponse(string inputLine)
        {
            var ticker = inputLine.Substring(1);
            var responses = new List<string>();
            if (string.IsNullOrWhiteSpace(ticker))
            {
                responses.Add("Not found");
            }
            else
            {
                var requestTime = DateTime.Now;
                if (_tickerTracker.ContainsKey(ticker))
                {
                    if (requestTime.Subtract(_tickerTracker[ticker]) < _norepeat)
                    {
                        return null;
                    }
                    _tickerTracker[ticker] = requestTime;
                }
                else
                {
                    _tickerTracker.Add(ticker, requestTime);
                }
                var pricing = YahooFinanceApi.Request(new[] { ticker });

                foreach (var yahooFinanceApiData in pricing)
                {
                    if (yahooFinanceApiData.Symbol.Equals("N/A", StringComparison.CurrentCultureIgnoreCase))
                    {
                        responses.Add("Ticker not found!");
                    }
                    else
                    {
                        responses.Add(
                            string.Format("{0} ({1}) - {2:HH:mm:ss tt} - {3:C} ({4}) | Op: {5:C} | Lo-Hi {6:C}-{7:C}",
                                yahooFinanceApiData.Name,
                                ticker.ToUpper(),
                                requestTime,
                                yahooFinanceApiData.LastTradePrice,
                                yahooFinanceApiData.PercentChange,
                                yahooFinanceApiData.Open,
                                yahooFinanceApiData.DayLow,
                                yahooFinanceApiData.DayHigh
                                ));
                    }
                }
            }
            return responses;
        }
    }
}