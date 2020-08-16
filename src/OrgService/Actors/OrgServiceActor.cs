using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using Common.Actors;
using Common.Actors.Messages;
using Shared.SystemMessages;

namespace OrgService.Actors
{
    public sealed class OrgServiceActor : ReceivePersistentActor
    {
        private readonly ActorSelection       _dispatcher;
        private readonly IActorRef            _consoleWriter;
        private          OrgServiceActorState _state;

        public OrgServiceActor( ActorSelection dispatcher, IActorRef consoleWriter )
        {
            _dispatcher    = dispatcher;
            _consoleWriter = consoleWriter;
            _state         = new OrgServiceActorState();
            PersistenceId  = "OrgService";
            Command<CreateActorForOrganization>( OnCreate );
            Recover<SnapshotOffer>( offer =>
            {
                _state = (OrgServiceActorState) offer.Snapshot;
                _state.Items.ForEach( OnCreate );
            } );
        }

        protected override void PreStart()
        {
            Context.GetLogger().Info( "OrgService address: {0}", Self.Path.ToStringWithAddress() );
        }

        private void OnCreate( CreateActorForOrganization msg )
        {
            _consoleWriter.Tell( WriteToConsole.Info( @$"OrgService: Получил комманду создать актора для организации {msg.OrganizationId}" ) );
            var childName = $"org-{msg.OrganizationId}";
            var child     = Context.Child( childName );
            if ( child.IsNobody() )
            {
                _consoleWriter.Tell( WriteToConsole.Info( @$"OrgService: Создаем актора для органзации {msg.OrganizationId} с цветом {msg.Color}" ) );
                var props = Props.Create( () => new OrganizationActor( msg.OrganizationId, msg.Color, _consoleWriter ) );
                child = Context.ActorOf( props, childName );
                _dispatcher.Tell( new RegisterOrganizationActor( msg.OrganizationId, child ) );
                if ( IsRecovering ) return; // Если восстанавливаем состояние, то дальнейшее пропускаем
                _consoleWriter.Tell( WriteToConsole.Success( @"OrgService: Актор создан" ) );
                Sender.Tell( new CreateActorForOrganizationResult( true, Array.Empty<string>() ) );
                var newState = _state.Add( msg );
                SaveSnapshot( newState );
                _state = newState;
            }
            else if ( ! IsRecovering )
            {
                _consoleWriter.Tell( WriteToConsole.Error( @$"OrgService: Актор организации {msg.OrganizationId} уже существует" ) );
                Sender.Tell( new CreateActorForOrganizationResult( false, new[] { @"Актор организации уже существует" } ) );
            }
        }

        public override string PersistenceId { get; }
    }

    public sealed class OrgServiceActorState
    {
        public ImmutableList<CreateActorForOrganization> Items { get; }

        public OrgServiceActorState( ImmutableList<CreateActorForOrganization> items )
        {
            Items = items;
        }

        public OrgServiceActorState()
        {
            Items = ImmutableList<CreateActorForOrganization>.Empty;
        }


        public OrgServiceActorState Add( CreateActorForOrganization item ) => new OrgServiceActorState( Items.Add( item ) );

        public OrgServiceActorState Remove( CreateActorForOrganization item ) =>
            new OrgServiceActorState( Items.RemoveAll( i => i.OrganizationId == item.OrganizationId && i.Color == item.Color ) );
    }
}