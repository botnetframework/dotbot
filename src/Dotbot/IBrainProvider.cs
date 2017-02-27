namespace Dotbot
{
    public interface IBrainProvider
    {
        void Connect();
        void Disconnect();

        string Get(string key);
        void Set(string key, string data);
    }
}
