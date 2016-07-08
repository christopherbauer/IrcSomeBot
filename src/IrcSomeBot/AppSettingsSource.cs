using System.Configuration;

namespace IrcSomeBot
{
    public class AppSettingsSource : ISettingsSource
    {
        private readonly AppSettingsReader _reader = new AppSettingsReader();

        public T GetValue<T>(string key)
        {
            return (T) _reader.GetValue(key, typeof (T));
        }
    }
}