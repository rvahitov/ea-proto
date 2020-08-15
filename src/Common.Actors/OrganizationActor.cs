using System;
using Akka.Actor;
using Common.Actors.Messages;
using Shared.Messages;

namespace Common.Actors
{
    public sealed class OrganizationActor : ReceiveActor
    {
        public OrganizationActor(string organizationId, string color, IActorRef consoleWriter)
        {
            Receive<GetColor>(msg =>
            {
                consoleWriter.Tell(WriteToConsole.Info($"Organization {organizationId}: received message GetColor"));
                Sender.Tell(new GetColorResponse(color, true, Array.Empty<string>()));
                consoleWriter.Tell(WriteToConsole.Success($"Organization {organizationId}: sent reply with color {color}"));
            });
        }
    }
}