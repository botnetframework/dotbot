namespace Dotbot
{
    public interface IStartup
    {
        string FriendlyName { get; }

        void Start();
    }
}
