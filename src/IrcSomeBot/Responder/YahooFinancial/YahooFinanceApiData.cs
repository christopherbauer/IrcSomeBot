namespace IrcSomeBot.Responder.YahooFinancial
{
    public class YahooFinanceApiData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal Open { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Change { get; set; }
        public string PercentChange { get; set; }
        public decimal DayLow { get; set; }
        public decimal DayHigh { get; set; }
        public string AfterHoursChange { get; set; }
        public decimal Volume { get; set; }
        public string PE { get; set; }
        public string WeekRange52 { get; set; }
        public decimal LastTradePrice { get; set; }
    }
}