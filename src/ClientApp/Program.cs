using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using ClientApp.Actors;
using ClientApp.Actors.Messages;

namespace ClientApp
{
    internal static class Program
    {
        private static void Main()
        {
            var config = ConfigurationFactory.ParseString( File.ReadAllText( "akka.conf" ) );

            using var system = ActorSystem.Create( "manager-system", config );
            var       props  = Props.Create( () => new ApplicationActor() );
            var       app    = system.ActorOf( props, "Application" );
            app.Tell( new ApplicationStart() );
            Console.CancelKeyPress += ( sender, eventArgs ) =>
            {
                // ReSharper disable AccessToDisposedClosure
                system.Terminate();
                // ReSharper restore AccessToDisposedClosure
                eventArgs.Cancel = true;
            };
            system.WhenTerminated.Wait();
        }
    }
}