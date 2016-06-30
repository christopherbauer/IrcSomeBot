using System;

namespace IrcSomeBot
{
    public class IrcMessage
    {
        public static IrcMessage ParseInput(string inputLine)
        {
            var inputParts = inputLine.Split(new[] {" :"}, StringSplitOptions.RemoveEmptyEntries);
            string message = null;
            if (inputParts.Length == 2)
            {
                message = inputParts[1];
            }
            var protocolParts = inputParts[0].Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            var logonDetails = protocolParts[0].Split(new [] {"!"},StringSplitOptions.RemoveEmptyEntries);
            var messageType = protocolParts[1];
            string target = null;
            if (protocolParts.Length == 3)
            {
                target = protocolParts[2];
            }

            string username = null, gateway;
            if (logonDetails.Length == 1)
            {
                gateway = logonDetails[0];
            }
            else
            {
                username = logonDetails[0].Substring(1);
                gateway = logonDetails[1];
            }
            return new IrcMessage(username, gateway, target, message);
        }

        public IrcMessage(string sender, string gateway, string target, string message)
        {
            Sender = sender;
            Gateway = gateway;
            Target = target;
            Message = message;
        }

        public string Message { get; }
        public string Sender { get; }
        public string Gateway { get; }
        public string Target { get; }
    }
}