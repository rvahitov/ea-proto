using System;
using Akka.Actor;
using Akka.Event;
using Common.Actors;
using Common.Actors.Messages;
using Shared.SystemMessages;

namespace OrgService.Actors
{
    public sealed class OrgServiceActor : ReceiveActor
    {
        private readonly ActorSelection _dispatcher;
        private readonly IActorRef _consoleWriter;

        public OrgServiceActor(ActorSelection dispatcher, IActorRef consoleWriter)
        {
            _dispatcher = dispatcher;
            _consoleWriter = consoleWriter;
            Receive<CreateActorForOrganization>(OnCreate);
        }

        protected override void PreStart()
        {
            Context.GetLogger().Info("OrgService address: {0}", Self.Path.ToStringWithAddress());
        }

        private void OnCreate(CreateActorForOrganization msg)
        {
            _consoleWriter.Tell(WriteToConsole.Info(@$"OrgService: Получил комманду создать актора для организации {msg.OrganizationId}"));
            var childName = $"org-{msg.OrganizationId}";
            var child = Context.Child(childName);
            if (child.IsNobody())
            {
                _consoleWriter.Tell(WriteToConsole.Info(@$"OrgService: Создаем актора для органзации {msg.OrganizationId} с цветом {msg.Color}"));
                var props = Props.Create(() => new OrganizationActor(msg.OrganizationId, msg.Color, _consoleWriter));
                child = Context.ActorOf(props, childName);
                _dispatcher.Tell(new RegisterOrganizationActor(msg.OrganizationId, child));
                _consoleWriter.Tell(WriteToConsole.Success(@"OrgService: Актор создан"));
                Sender.Tell(new CreateActorForOrganizationResult(true, Array.Empty<string>()));
            }
            else
            {
                _consoleWriter.Tell(WriteToConsole.Error(@$"OrgService: Актор организации {msg.OrganizationId} уже существует"));
                Sender.Tell(new CreateActorForOrganizationResult(false, new[] {@"Актор организации уже существует"}));
            }
        }
    }
}