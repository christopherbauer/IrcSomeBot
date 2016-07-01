using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using IrcSomeBot.Responder;

namespace IrcSomeBot
{
    public class IrcBot
    {

        private readonly int _port;
        private readonly string _channel;
        private readonly string _username;
        private readonly string _server;
        // User information defined in RFC 2812 (Internet Relay Chat: Client Protocol) is sent to irc server 
        private const string User = "USER CSharpBot:I'm a C# irc bot";
        // StreamWriter is declared here so that PingSender can access it
        public static StreamWriter Writer;
        private readonly IList<IResponder> _responders;

        public IrcBot(string server, int port, string channel, string username)
        {
            _server = server;
            _port = port;
            _channel = channel;
            _username = username;
            _responders = new List<IResponder>();
        }

        public void Initialize()
        {
            string inputLine = null;

            try
            {
                var irc = new TcpClient(_server, _port);
                var stream = irc.GetStream();
                var reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
                // Start PingSender thread
                var ping = new PingSender(_server);
                ping.Start();
                Writer.WriteLine(User);
                Writer.Flush();
                Writer.WriteLine("NICK " + _username);
                Writer.Flush();
                Writer.WriteLine("JOIN " + _channel);
                Writer.Flush();
                while (true)
                {
                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        Debug.WriteLine(inputLine);
                        var garbage = inputLine.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
                        if (garbage.Length > 1)
                        {
                            var ircMessage = IrcMessage.ParseInput(inputLine);
                            var @private = ircMessage.Target == _username;

                            foreach (var responder in _responders)
                            {
                                if (responder.HasResponse(ircMessage.Message))
                                {
                                    var responses = responder.GetResponse(ircMessage);
                                    if (responses != null)
                                    {
                                        foreach (var response in responses)
                                        {
                                            var ircResponse = string.Format("PRIVMSG {0} :{1}",
                                                @private ? ircMessage.Sender : _channel,
                                                response);
                                            Console.WriteLine("Input: {0}", inputLine);
                                            Console.WriteLine("Response: {0}", ircResponse);
                                            Writer.WriteLine(ircResponse);
                                        }
                                        Writer.Flush();
                                    }
                                }
                            }
                        }
                    }
                    // Close all streams
                    Writer.Close();
                    reader.Close();
                    irc.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Source: {0}",inputLine);
                Initialize();
            }
        }

        public void LoadResponder(IResponder responder)
        {
            _responders.Add(responder);
        }
    }
}