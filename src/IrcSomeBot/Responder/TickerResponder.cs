using System;
using System.Collections.Generic;
using System.Threading;

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

        public bool HasResponse(IrcMessage ircMessage)
        {
            return ircMessage.Message != null && ircMessage.Message.StartsWith(",");
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
                
                var @private = ircMessage.Target == _username;
                var tickerTrackerRecord = new TickerTrackerRecord(ticker, @private ? ircMessage.Sender : _channel);
                var requestDifference = new TimeSpan();
                var repeat = false;
                if (_tickerTracker.ContainsKey(tickerTrackerRecord))
                {
                    requestDifference = requestTime.Subtract(_tickerTracker[tickerTrackerRecord]);
                    repeat = requestDifference < _norepeat;

                    if (!repeat)
                    {
                       _tickerTracker[tickerTrackerRecord] = requestTime;
                    }
                }
                else
                {
                    _tickerTracker.Add(tickerTrackerRecord, requestTime);
                }

                foreach (var pricingData in _stockTickerDataSource.GetPricingData(ticker, requestTime))
                {
                    if (repeat)
                    {
                        responses.Add(string.Format(@"PRIVMSG {0} : I am configured to only post tickers every {3:mm\:ss}. Ticker was requested {1:mm\:ss} ago. Please wait {2:mm\:ss} before requesting again.", ircMessage.Sender, requestDifference, _norepeat.Subtract(requestDifference), _norepeat));
                    }
                    else
                    {
                        responses.Add(string.Format(@"PRIVMSG {0} :{1}", @private ? ircMessage.Sender : _channel, pricingData));
                    }
                }
            }
            return responses;
        }
    }
}