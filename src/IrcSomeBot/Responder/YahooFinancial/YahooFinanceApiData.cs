namespace IrcSomeBot.Responder.YahooFinancial
{
    public class YahooFinanceApiData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Bid { get; set; }
        public string Ask { get; set; }
        public string Open { get; set; }
        public string PreviousClose { get; set; }
        public string Change { get; set; }
        public string PercentChange { get; set; }
        public string DayLow { get; set; }
        public string DayHigh { get; set; }
        public string AfterHoursChange { get; set; }
        public string Volume { get; set; }
        public string PE { get; set; }
        public string WeekRange52 { get; set; }
        public string LastTradePrice { get; set; }
        public string LastTradeTime { get; set; }
        public string LastTradeDate { get; set; }
    }
}