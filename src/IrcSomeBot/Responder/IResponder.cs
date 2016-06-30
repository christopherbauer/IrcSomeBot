using System.Collections.Generic;

namespace IrcSomeBot.Responder
{
    public interface IResponder
    {
        bool HasResponse(string inputLine);
        List<string> GetResponse(string inputLine);
    }
}