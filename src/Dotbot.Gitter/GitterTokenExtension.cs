using Bayeux;

namespace Dotbot.Gitter
{
    internal sealed class GitterTokenExtension : BayeuxProtocolExtension
    {
        private readonly string _token;

        public GitterTokenExtension(string token) 
            : base("token")
        {
            _token = token;
        }

        public override bool TryExtendOutgoing(IBayeuxMessage message, out object extension)
        {
            extension = _token;
            return true;
        }
    }
}
