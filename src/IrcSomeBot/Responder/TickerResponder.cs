using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public class TickerResponder : IResponder
    {
        private readonly IStockTickerDataSource _stockTickerDataSource;
        private readonly Dictionary<TickerTrackerRecord, DateTime> _tickerTracker;
        private readonly TimeSpan _norepeat;
        private readonly string _username;
        private readonly string _channel;

        public TickerResponder(IStockTickerDataSource stockTickerDataSource, TimeSpan norepeat, string username, string channel)
        {
            _stockTickerDataSource = stockTickerDataSource;
            _norepeat = norepeat;
            _username = username;
            _channel = channel;
            _tickerTracker = new Dictionary<TickerTrackerRecord, DateTime>();
        }

        public bool HasResponse(string inputLine)
        {
            return inputLine.StartsWith(",");
        }

        public List<string> GetResponse(IrcMessage ircMessage)
        {
            var ticker = ircMessage.Message.Substring(1);
            var responses = new List<string>();
            if (string.IsNullOrWhiteSpace(ticker))
            {
                responses.Add("Not found");
            }
            else
            {
                var requestTime = DateTime.Now;
                var tickerTrackerRecord = new TickerTrackerRecord(ticker, ircMessage.Target == _username ? ircMessage.Sender : _channel);
                if (_tickerTracker.ContainsKey(tickerTrackerRecord))
                {
                    if (requestTime.Subtract(_tickerTracker[tickerTrackerRecord]) < _norepeat)
                    {
                        return null;
                    }
                    _tickerTracker[tickerTrackerRecord] = requestTime;
                }
                else
                {
                    _tickerTracker.Add(tickerTrackerRecord, requestTime);
                }
                
                responses.AddRange(_stockTickerDataSource.GetPricingData(ticker, requestTime));
            }
            return responses;
        }
    }
}