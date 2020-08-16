using System;
using Akka.Actor;
using ClientApp.Actors.Messages;

namespace ClientApp.Actors
{
    public sealed class ConsoleReaderActor : ReceiveActor
    {
        public ConsoleReaderActor()
        {
            Receive<ReadConsole>( cmd =>
            {
                var line = Console.ReadLine();
                cmd.SendTo.Tell( new ConsoleReadResult( line ) );
            } );
        }
    }
}