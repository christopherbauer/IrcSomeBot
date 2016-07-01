using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public class KickResponder : IResponder
    {
        private readonly string _username;
        private readonly string _channel;

        public KickResponder(string username, string channel)
        {
            _username = username;
            _channel = channel;
        }

        public bool HasResponse(IrcMessage ircMessage)
        {
            return ircMessage.Message.Contains("KICK") && ircMessage.Message.Contains(_username);
        }

        public List<string> GetResponse(IrcMessage ircMessage)
        {
            return new List<string> {string.Format("/j {0}", _channel)};
        }
    }
}