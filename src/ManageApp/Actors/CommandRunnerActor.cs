using System.Linq;
using Akka.Actor;

namespace ManageApp.Actors
{
    public sealed class CommandRunnerActor : ReceiveActor
    {
        private readonly IActorRef _consoleWriter;
        private readonly IActorRef _consoleReader;
        private readonly ActorSelection[] _orgServices;

        public CommandRunnerActor(IActorRef consoleWriter, IActorRef consoleReader)
        {
            _consoleWriter = consoleWriter;
            _consoleReader = consoleReader;
            var addressList = Context.System.Settings.Config.GetStringList("app.org-services");
            _orgServices = addressList.Select(a => Context.ActorSelection(a)).ToArray();
        }
    }
}