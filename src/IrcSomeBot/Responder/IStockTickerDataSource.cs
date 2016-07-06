using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public interface IStockTickerDataSource
    {
        IEnumerable<string> GetPricingData(string ticker);
    }
}