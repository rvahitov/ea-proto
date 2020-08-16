using Akka.Actor;
using ClientApp.Actors.Messages;
using Common.Actors;
using Common.Actors.Messages;

namespace ClientApp.Actors
{
    public sealed class ApplicationActor : ReceiveActor
    {
        public ApplicationActor()
        {
            Receive<ApplicationStart>( _ =>
            {
                var consoleWriter = Context.ActorOf( Props.Create( () => new ConsoleWriterActor() ) );
                var consoleReader = Context.ActorOf( Props.Create( () => new ConsoleReaderActor() ) );
                var commandRunner = Context.ActorOf( Props.Create( () => new CommandRunnerActor( consoleWriter, consoleReader ) ) );
                var commandParser = Context.ActorOf( Props.Create( () => new CommandParserActor( consoleWriter, commandRunner ) ) );
                PrintInformation( consoleWriter );
                consoleReader.Tell( new ReadConsole( commandParser ) );
            } );
        }

        private static void PrintInformation( IActorRef consoleWriter )
        {
            consoleWriter.Tell( WriteToConsole.Success( "Клиент запущен." ) );
            consoleWriter.Tell( WriteToConsole.Info( "Доступны следующие команды:" ) );
            consoleWriter.Tell( WriteToConsole.Info( "\tGetFrom [OrganizationId] - для отправки запроса актору организации" ) );
            consoleWriter.Tell( WriteToConsole.Info( "\tExit - для завершения работы клиента" ) );
            consoleWriter.Tell( WriteToConsole.Info( "Для выхода из программы введите Exit или Ctrl-C" ) );
        }
    }
}