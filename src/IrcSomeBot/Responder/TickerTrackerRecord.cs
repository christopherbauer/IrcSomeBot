namespace IrcSomeBot.Responder
{
    public struct TickerTrackerRecord
    {
        public TickerTrackerRecord(string tickerSymbol, string requestSource)
        {
            TickerSymbol = tickerSymbol;
            RequestSource = requestSource;
        }

        public string TickerSymbol { get; }
        public string RequestSource { get; }
    }
}