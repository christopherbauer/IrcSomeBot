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
        private readonly IOutputInterface _outputInterface;
        private readonly string _server;
        // User information defined in RFC 2812 (Internet Relay Chat: Client Protocol) is sent to irc server 
        private const string User = "USER IrcSomeBot:I'm an IRC bot";
        // StreamWriter is declared here so that PingSender can access it
        private readonly IList<IResponder> _responders;

        public IrcBot(IOutputInterface outputInterface, string server, int port, string channel, string username)
        {
            _outputInterface = outputInterface;
            _server = server;
            _port = port;
            _channel = channel;
            _username = username;
            _responders = new List<IResponder>();
        }

        public void Initialize()
        {
            Console.WriteLine("Initialize Connection");
            string inputLine = null;
#if !DEBUG
            try
            {
#endif
            using (var irc = new TcpClient(_server, _port))
            {

                var stream = irc.GetStream();
                var reader = new StreamReader(stream);
                _outputInterface.SetInterface(new StreamWriter(stream));
                // Start PingSender thread
                var ping = new PingSender(_outputInterface, _server);
                ping.Start();
                _outputInterface.WriteLine(User);
                _outputInterface.WriteLine("NICK " + _username);
                _outputInterface.WriteLine("JOIN " + _channel);
                while (true)
                {
                    while ((inputLine = reader.ReadLine()) != null)
                    {
                        Debug.WriteLine(inputLine);
                        var garbage = inputLine.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
                        if (garbage.Length > 1)
                        {
                            var ircMessage = IrcMessage.ParseInput(inputLine);

                            foreach (var responder in _responders)
                            {
                                if (responder.HasResponse(ircMessage))
                                {
                                    var responses = responder.GetResponse(ircMessage);
                                    if (responses != null)
                                    {
                                        foreach (var response in responses)
                                        {
                                            Console.WriteLine("Input: {0}", inputLine);
                                            Console.WriteLine("Response: {0}", response);
                                            _outputInterface.WriteLine(response);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // Close all streams
                    _outputInterface.Close();
                    reader.Close();
                    irc.Close();
                }
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Source: {0}",inputLine);
                Initialize();
            }
#endif
        }

        public void LoadResponder(IResponder responder)
        {
            _responders.Add(responder);
        }
    }
}