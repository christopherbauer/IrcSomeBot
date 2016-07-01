using System;
using System.IO;

namespace IrcSomeBot
{
    public interface IOutputInterface
    {
        void WriteLine(string message);
        void Close();
        void SetInterface(object @interface);
    }
}