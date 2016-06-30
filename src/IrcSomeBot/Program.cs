using System;
using System.Configuration;
using IrcSomeBot.Responder;

namespace IrcSomeBot
{
    public delegate void CommandReceived(string ircCommand);

    class Program
    {
        static void Main(string[] args)
        {
            var reader = new AppSettingsReader();
            var server = reader.GetValue("server", typeof(string)).ToString();
            var port = Convert.ToInt32(reader.GetValue("port", typeof(int)));
            var channel = reader.GetValue("channel", typeof(string)).ToString();
            var username = reader.GetValue("username", typeof(string)).ToString();

            var bot = new IrcBot(server, port, channel, username);
            bot.LoadResponder(new TickerResponder(TimeSpan.FromSeconds(30)));
            bot.LoadResponder(new KickResponder(username, channel));
            bot.Initialize();
        }
    }
}
