using System.Linq;
using Akka.Actor;
using Common.Actors;
using Common.Actors.Messages;
using ManageApp.Actors.Message;

namespace ManageApp.Actors
{
    public sealed class ApplicationActor : ReceiveActor
    {
        public ApplicationActor()
        {
            Receive<StartApplication>( _ =>
            {
                var consoleWriter = Context.ActorOf( Props.Create( () => new ConsoleWriterActor() ) );
                var commandRunner = Context.ActorOf( Props.Create( () => new CommandRunnerActor( consoleWriter ) ) );
                var commandParser = Context.ActorOf( Props.Create( () => new CommandParserActor( commandRunner, consoleWriter ) ) );
                var commandReader = Context.ActorOf( Props.Create( () => new CommandReaderActor( commandParser ) ) );
                PrintInformation( consoleWriter, commandReader );
            } );
        }

        private static void PrintInformation( IActorRef consoleWriter, IActorRef commandReader )
        {
            var addressList = Context.System.Settings.Config.GetStringList( "akka.app.org-services" );
            consoleWriter.Tell( WriteToConsole.Success( "Manager запущен." ) );
            consoleWriter.Tell( WriteToConsole.Info( @"Доступны следующие сервисы организаций:" ) );
            foreach ( var (index, address) in addressList.Select( ( a, i ) => (i + 1, a) ) )
            {
                consoleWriter.Tell( WriteToConsole.Info( $"\t{index}. {address}" ) );
            }

            consoleWriter.Tell( WriteToConsole.Info( "Доступны следующие команды:" ) );
            consoleWriter.Tell( WriteToConsole.Info( "\tAddTo [ServiceNumber] [OrganizationId] [Color] - для добавления организации в указанный сервис" ) );
            consoleWriter.Tell( WriteToConsole.Info( "\tExit - для завершения работы сервиса" ) );
            consoleWriter.Tell( WriteToConsole.Info( "Для выхода из программы введите Exit или Ctrl-C" ) );
            commandReader.Tell( "Read" );
        }
    }
}