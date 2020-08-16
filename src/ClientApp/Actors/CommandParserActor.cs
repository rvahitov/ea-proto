using System.Text.RegularExpressions;
using Akka.Actor;
using ClientApp.Actors.Messages;
using Common.Actors.Messages;
using Shared.Messages;

namespace ClientApp.Actors
{
    public sealed class CommandParserActor : ReceiveActor
    {
        private const           string ExitCommand     = "exit";
        private static readonly Regex  GetCommandRegex = new Regex( @"^\s*GetFrom\s+(\w+)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase );

        public CommandParserActor( IActorRef consoleWriter, IActorRef commandRunner )
        {
            Receive<ConsoleReadResult>( msg =>
            {
                if ( string.IsNullOrEmpty( msg.Line ) )
                {
                    consoleWriter.Tell( WriteToConsole.Error( "Введите команду" ) );
                    Sender.Tell( new ReadConsole( Self ) );
                    return;
                }

                if ( string.Equals( ExitCommand, msg.Line ) )
                {
                    Context.System.Terminate();
                    return;
                }

                var getFromMatch = GetCommandRegex.Match( msg.Line );
                if ( getFromMatch.Success )
                {
                    commandRunner.Tell( new GetColor( getFromMatch.Groups[1].Value ) );
                    return;
                }

                consoleWriter.Tell( WriteToConsole.Error( "Неизвестная команда" ) );
            } );
        }
    }
}