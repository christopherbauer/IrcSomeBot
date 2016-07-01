using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public interface IResponder
    {
        bool HasResponse(IrcMessage ircMessage);
        List<string> GetResponse(IrcMessage ircMessage);
    }
}