using System;
using System.IO;

namespace IrcSomeBot
{
    public class StreamWriterWrapper : IOutputInterface
    {
        private StreamWriter Writer { get; set; }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
            Writer.WriteLine(message);
            Writer.Flush();
        }

        public void Close()
        {
            Writer.Close();
        }

        public void SetInterface(object @interface)
        {
            Writer = (StreamWriter)@interface;
        }
    }
}