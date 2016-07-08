using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder.YahooFinancial
{
    public class YahooApiDatasource : IStockTickerDataSource
    {
        private IDictionary<string, string> _nameTransformDictionary;
        private readonly ISettingsSource _settingsSource;

        public YahooApiDatasource(ISettingsSource settingsSource)
        {
            _settingsSource = settingsSource;
            Initialize();
        }

        private void Initialize()
        {
            _nameTransformDictionary = new Dictionary<string, string>();

            var value = _settingsSource.GetValue<string>("quote-name-transforms");

            var nameTransforms = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var nameTransform in nameTransforms)
            {
                _nameTransformDictionary.Add(nameTransform.Split(':')[0], nameTransform.Split(':')[1]);
            }
        }

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
                            _nameTransformDictionary.ContainsKey(yahooFinanceApiData.Name) ? _nameTransformDictionary[yahooFinanceApiData.Name] : yahooFinanceApiData.Name,
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