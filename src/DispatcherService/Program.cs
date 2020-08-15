using System;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Common.Actors;
using Common.Actors.Messages;

namespace DispatcherService
{
    class Program
    {
        private static void Main()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));

            using var system = ActorSystem.Create("dispatcher-system", config);

            var consoleWriterProps = Props.Create(() => new ConsoleWriteActor());
            var consoleWriter = system.ActorOf(consoleWriterProps, "ConsoleWriter");

            var dispatcherProps = Props.Create(() => new DispatcherActor(consoleWriter, "Dispatcher"));
            system.ActorOf(dispatcherProps, "Dispatcher");
            consoleWriter.Tell(WriteToConsole.Success(@"Диспетчер запущен."));
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