using System;
using Akka.Actor;
using ManageApp.Actors.Message;

namespace ManageApp.Actors
{
    public sealed class CommandReaderActor : UntypedActor
    {
        private readonly IActorRef _commandParser;

        public CommandReaderActor(IActorRef commandParser)
        {
            _commandParser = commandParser;
        }

        protected override void OnReceive(object message)
        {
            var command = Console.ReadLine();
            _commandParser.Tell(new ParseCommand(command));
        }
    }
}