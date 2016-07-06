using System;
using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public class JoinResponder : IResponder
    {
        private const string CommandChannel = "!command channel";

        public bool HasResponse(IrcMessage ircMessage)
        { 
            return ircMessage.Message != null && ircMessage.Message.StartsWith(CommandChannel, StringComparison.CurrentCultureIgnoreCase);
        }

        public List<string> GetResponse(IrcMessage ircMessage)
        {
            return new List<string> {string.Format("JOIN {0}", ircMessage.Message.Substring(CommandChannel.Length))};
        }
    }
}