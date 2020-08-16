using System;
using System.Linq;
using Akka.Actor;
using Common.Actors.Messages;
using ManageApp.Actors.Message;
using Shared.SystemMessages;

namespace ManageApp.Actors
{
    public sealed class CommandRunnerActor : ReceiveActor
    {
        private readonly IActorRef        _consoleWriter;
        private readonly ActorSelection[] _orgServices;

        public CommandRunnerActor( IActorRef consoleWriter )
        {
            _consoleWriter = consoleWriter;
            var addressList = Context.System.Settings.Config.GetStringList( "akka.app.org-services" );
            _orgServices = addressList.Select( a => Context.ActorSelection( a + "/user/OrgService" ) ).ToArray();

            Receive<AddActor>( OnAddActor );

            Receive<ExitApplication>( _ => Context.System.Terminate() );

            Receive<CreateActorForOrganizationResult>( OnCreateResult );
        }

        private void OnCreateResult( CreateActorForOrganizationResult msg )
        {
            var write = msg.IsSuccess
                ? WriteToConsole.Success( "Организация создана" )
                : WriteToConsole.Error( $"Организация не создана. Сервер вернул ошибки: {string.Join( $"{Environment.NewLine}\t\t", msg.Errors )}" );
            _consoleWriter.Tell( write );
        }

        private void OnAddActor( AddActor cmd )
        {
            var serverNumber = cmd.ServerNumber - 1;
            if ( serverNumber < 0 || serverNumber >= _orgServices.Length )
            {
                _consoleWriter.Tell( WriteToConsole.Error( "Недопустимый номер сервера" ) );
                return;
            }

            _orgServices[serverNumber].Tell( new CreateActorForOrganization( cmd.Organization, cmd.Color ) );
            Sender.Tell( "Read" );
        }
    }
}