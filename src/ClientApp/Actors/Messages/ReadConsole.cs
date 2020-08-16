using Akka.Actor;

namespace ClientApp.Actors.Messages
{
    public sealed class ReadConsole
    {
        public ReadConsole( IActorRef sendTo )
        {
            SendTo = sendTo;
        }

        public IActorRef SendTo { get; }
    }
}