using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using Common.Actors.Messages;
using Shared.Messages;
using Shared.SystemMessages;

namespace DispatcherService
{
    public sealed class DispatcherActor : ReceivePersistentActor
    {
        public DispatcherActor(IActorRef consoleWriter, string id)
        {
            PersistenceId = id;
            var state = new DispatcherState();

            Recover<SnapshotOffer>(offer => { state = (DispatcherState) offer.Snapshot; });
            Command<RegisterOrganizationActor>(command =>
            {
                var writeCommand = WriteToConsole.Info(@$"Получил запрос регистрации актора для организации {command.OrganizationId}. Актор: {command.OrganizationActor}");
                consoleWriter.Tell(writeCommand);
                var map = new OrganizationActorMap(command.OrganizationId, command.OrganizationActor);
                if (state.Contains(map))
                {
                    consoleWriter.Tell(WriteToConsole.Error(@$"Актор уже зарегистрирован: {command.OrganizationActor}"));
                    return;
                }

                var newState = state.Add(map);
                SaveSnapshot(newState);
                state = newState;
                consoleWriter.Tell(WriteToConsole.Success(@"Актор зарегистрирован успешно"));
            });

            Command<UnregisterOrganizationActor>(command =>
            {
                var writeCommand =
                    WriteToConsole.Info(@$"Получил запрос на снятие с регистрации актора для организации {command.OrganizationId}. Актор: {command.OrganizationActor}");
                consoleWriter.Tell(writeCommand);
                var map = new OrganizationActorMap(command.OrganizationId, command.OrganizationActor);
                if (!state.Contains(map))
                {
                    consoleWriter.Tell(WriteToConsole.Error(@$"Актор не зарегистрирован: {command.OrganizationActor}"));
                    return;
                }

                var newState = state.Remove(map);
                SaveSnapshot(newState);
                state = newState;
                consoleWriter.Tell(WriteToConsole.Success(@"Актор снят с регистрации успешно"));
            });

            Command<GetColor>(query =>
            {
                var maps = state.FindForOrganization(query.OrganizationId);
                if (maps.Length == 0)
                {
                    Sender.Tell(new GetColorResponse(null, false, new[] {@"Организация не зарегистрирована"}));
                }
                else
                {
                    maps[0].Actor.Forward(query);
                }
            });
        }

        public override string PersistenceId { get; }

        protected override void PreStart()
        {
            Context.GetLogger().Info("Dispatcher address: {0}", Self.Path.ToStringWithAddress());
        }
    }
}