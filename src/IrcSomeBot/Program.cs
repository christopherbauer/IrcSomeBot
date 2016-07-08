using System;
using System.Configuration;
using IrcSomeBot.Responder;
using IrcSomeBot.Responder.YahooFinancial;

namespace IrcSomeBot
{
    public delegate void CommandReceived(string ircCommand);

    class Program
    {
        static void Main(string[] args)
        {
            var appSettingsSource = new AppSettingsSource();
            var server = appSettingsSource.GetValue<string>("server");
            var port = appSettingsSource.GetValue<int>("port");
            var channel = appSettingsSource.GetValue<string>("channel");
            var username = appSettingsSource.GetValue<string>("username");

            var bot = new IrcBot(new StreamWriterWrapper(), server, port, channel, username);
            bot.LoadResponder(new QuoteResponder(new YahooApiDatasource(appSettingsSource),appSettingsSource, TimeSpan.FromSeconds(30), username, channel));
            bot.LoadResponder(new KickResponder(username, channel));
            bot.LoadResponder(new JoinResponder());
            bot.Initialize();
        }
    }
}
