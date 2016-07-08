using System;
using System.Collections.Generic;
using System.Linq;

namespace IrcSomeBot.Responder
{
    public class QuoteResponder : IResponder
    {
        private readonly IStockTickerDataSource _stockTickerDataSource;
        private readonly ISettingsSource _settingsSource;
        private Dictionary<TickerTrackerRecord, DateTime> _tickerTracker;
        private readonly TimeSpan _norepeat;
        private readonly string _username;
        private readonly string _channel;
        private IDictionary<string,string> _tickerTransformDictionary;

        public QuoteResponder(IStockTickerDataSource stockTickerDataSource, ISettingsSource settingsSource, TimeSpan norepeat, string username, string channel)
        {
            _stockTickerDataSource = stockTickerDataSource;
            _norepeat = norepeat;
            _username = username;
            _channel = channel;
            _settingsSource = settingsSource;
            Initialize();
        }

        private void Initialize()
        {
            _tickerTracker = new Dictionary<TickerTrackerRecord, DateTime>();
            _tickerTransformDictionary = new Dictionary<string, string>();

            var value = _settingsSource.GetValue<string>("quote-ticker-transforms");
            var tickerTransforms = value.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (tickerTransforms.Any())
            {
                foreach (var tickerTransform in tickerTransforms)
                {
                    _tickerTransformDictionary.Add(tickerTransform.Split(':')[0], tickerTransform.Split(':')[1]);
                }
            }
        }

        public bool HasResponse(IrcMessage ircMessage)
        {
            return ircMessage.Message != null && ircMessage.Message.StartsWith(",");
        }

        public List<string> GetResponse(IrcMessage ircMessage)
        {
            var ticker = ircMessage.Message.Substring(1);
            if (_tickerTransformDictionary.ContainsKey(ticker))
            {
                ticker = _tickerTransformDictionary[ticker];
            }

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

                foreach (var pricingData in _stockTickerDataSource.GetPricingData(ticker))
                {
                    if (repeat)
                    {
                        responses.Add(string.Format(@"PRIVMSG {0} : I am configured to only post tickers every {3:mm\:ss}. Ticker was requested {1:mm\:ss} ago. Please wait {2:mm\:ss} before requesting again.", ircMessage.Sender, requestDifference, _norepeat.Subtract(requestDifference), _norepeat));
                    }
                    else
                    {
                        responses.Add(string.Format(@"PRIVMSG {0} :{1}", @private ? ircMessage.Sender : ircMessage.Target, pricingData));
                    }
                }
            }
            return responses;
        }
    }
}