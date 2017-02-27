namespace Dotbot
{
    public interface IAdapter
    {
        string FriendlyName { get; }
        IBroker Broker { get; }
    }
}
