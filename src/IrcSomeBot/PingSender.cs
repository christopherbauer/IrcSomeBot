
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace IrcSomeBot
{
    /*
    * Class that sends PING to irc server every 15 seconds
    */

    public class PingSender
    {
        private readonly IOutputInterface _outputInterface;
        private readonly string _server;
        private readonly Thread _pingThread;
        // Empty constructor makes instance of Thread
        public PingSender(IOutputInterface outputInterface, string server)
        {
            _outputInterface = outputInterface;
            _server = server;
            _pingThread = new Thread(this.Run);
        }

        // Starts the thread
        public void Start()
        {
            _pingThread.Start();
        }
        // Send PING to irc server every 15 seconds
        public void Run()
        {
            while (true)
            {
                _outputInterface.WriteLine(string.Format("PING :{0}", _server));
                Thread.Sleep(15000);
            }
        }
    }
}
