namespace IrcSomeBot
{
    public interface ISettingsSource
    {
        T GetValue<T>(string key);
    }
}