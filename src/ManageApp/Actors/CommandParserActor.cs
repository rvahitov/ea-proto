using System;
using System.Text.RegularExpressions;
using Akka.Actor;
using Common.Actors.Messages;
using ManageApp.Actors.Message;

namespace ManageApp.Actors
{
    public sealed class CommandParserActor : ReceiveActor
    {
        private static readonly Regex     AddCommandRegex = new Regex( @"^\s*AddTo\s+(\d)\s+(\w+)\s+(\w+)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase );
        private readonly        IActorRef _commandRunner;
        private readonly        IActorRef _consoleWriter;

        public CommandParserActor( IActorRef commandRunner, IActorRef consoleWriter )
        {
            _commandRunner = commandRunner;
            _consoleWriter = consoleWriter;
            Receive<ParseCommand>( OnParse );
        }

        private void OnParse( ParseCommand msg )
        {
            if ( string.IsNullOrEmpty( msg.CommandText ) )
            {
                var error = WriteToConsole.Error( @"Введите команду" );
                _consoleWriter.Tell( error );
                Sender.Tell( "Read" );
                return;
            }

            if ( string.Equals( "Exit", msg.CommandText, StringComparison.OrdinalIgnoreCase ) )
            {
                _commandRunner.Forward( new ExitApplication() );
                return;
            }

            var addCommandMatch = AddCommandRegex.Match( msg.CommandText );
            if ( addCommandMatch.Success )
            {
                var serverNumber   = int.Parse( addCommandMatch.Groups[1].Value );
                var organizationId = addCommandMatch.Groups[2].Value;
                var color          = addCommandMatch.Groups[3].Value;
                _commandRunner.Forward( new AddActor( serverNumber, organizationId, color ) );
                return;
            }

            var error2 = WriteToConsole.Error( @$"Неизвестная команда {msg.CommandText}" );
            _consoleWriter.Tell( error2 );
            Sender.Tell( "Read" );
        }
    }
}