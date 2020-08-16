using System;
using Akka.Actor;
using ClientApp.Actors.Messages;
using Common.Actors.Messages;
using Shared.Messages;

namespace ClientApp.Actors
{
    public sealed class CommandRunnerActor : ReceiveActor
    {
        public CommandRunnerActor( IActorRef consoleWriter, IActorRef consoleReader )
        {
            var dispatcherAddress = Context.System.Settings.Config.GetString( "akka.app.dispatcher-address" ) + "/user/Dispatcher";
            var dispatcher        = Context.ActorSelection( dispatcherAddress );
            Receive<Response>( msg =>
            {
                var write = msg.ColorResponse.IsSuccess
                    ? WriteToConsole.Success( $"Ответ получен. Цвет - {msg.ColorResponse.Color}" )
                    : WriteToConsole.Error( $"Ответ получен. Ошибка: {string.Join( Environment.NewLine, msg.ColorResponse.Errors )}" );
                consoleWriter.Tell( write );
                consoleReader.Tell( new ReadConsole( msg.Sender ) );
            } );

            Receive<GetColor>( msg =>
            {
                var sender = Sender;
                dispatcher.Ask<GetColorResponse>( msg ).ContinueWith( t => new Response( t.Result, sender ) ).PipeTo( Self );
            } );
        }

        private sealed class Response
        {
            public Response( GetColorResponse colorResponse, IActorRef sender )
            {
                ColorResponse = colorResponse;
                Sender        = sender;
            }

            public GetColorResponse ColorResponse { get; }
            public IActorRef        Sender        { get; }
        }
    }
}