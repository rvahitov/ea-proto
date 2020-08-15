using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Common.Actors;
using Common.Actors.Messages;
using OrgService.Actors;

namespace OrgService
{
    class Program
    {
        private static void Main()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));

            using var system = ActorSystem.Create("organizations-system", config);

            var consoleWriterProps = Props.Create(() => new ConsoleWriteActor());
            var consoleWriter = system.ActorOf(consoleWriterProps, "ConsoleWriter");

            var dispatcherAddress = config.GetString("akka.app.dispatcher-address");
            var dispatcherSelection = system.ActorSelection(dispatcherAddress);

            var props = Props.Create(() => new OrgServiceActor(dispatcherSelection, consoleWriter));
            system.ActorOf(props, "OrgService");
            
            consoleWriter.Tell(WriteToConsole.Success(@"Сервис организаций запущен."));
            consoleWriter.Tell(WriteToConsole.Info(@"Для выхода нажмите Ctrl-C"));
            
            Console.CancelKeyPress += (sender, eventArgs) =>
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