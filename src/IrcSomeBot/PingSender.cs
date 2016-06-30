
using System.Threading;

namespace IrcSomeBot
{
    /*
    * Class that sends PING to irc server every 15 seconds
    */
    class PingSender
    {
        private readonly string _server;
        static string PING = "PING :";
        private Thread pingSender;
        // Empty constructor makes instance of Thread
        public PingSender(string server)
        {
            _server = server;
            pingSender = new Thread(this.Run);
        }

        // Starts the thread
        public void Start()
        {
            pingSender.Start();
        }
        // Send PING to irc server every 15 seconds
        public void Run()
        {
            while (true)
            {
                IrcBot.Writer.WriteLine(PING + _server);
                IrcBot.Writer.Flush();
                Thread.Sleep(15000);
            }
        }
    }
}
