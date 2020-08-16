using System;
using Akka.Actor;
using Common.Actors.Messages;

namespace Common.Actors
{
    public sealed class ConsoleWriterActor : ReceiveActor
    {
        public ConsoleWriterActor()
        {
            Receive<WriteToConsole>(msg =>
            {
                switch (msg.Message)
                {
                    case InfoMessage info:
                        Console.WriteLine(info.Text);
                        break;
                    case SuccessMessage success:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(success.Text);
                        Console.ResetColor();
                        break;
                    case ErrorMessage error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(error.Text);
                        Console.ResetColor();
                        break;
                }
            });
        }
    }
}