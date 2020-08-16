using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;

namespace DispatcherService
{
    public sealed class DispatcherState
    {
        public ImmutableList<OrganizationActorMap> Maps { get; }

        public DispatcherState(ImmutableList<OrganizationActorMap> maps)
        {
            Maps = maps;
        }

        public DispatcherState()
        {
            Maps = ImmutableList<OrganizationActorMap>.Empty;
        }

        public bool Contains(OrganizationActorMap map) => Maps.Contains(map);

        public DispatcherState Add(OrganizationActorMap map) =>
            new DispatcherState(Maps.Add(map));

        public DispatcherState Remove(OrganizationActorMap map) =>
            new DispatcherState(Maps.Remove(map));

        public OrganizationActorMap[] FindForOrganization(string organization) =>
            Maps.Where(m => m.Organization == organization).ToArray();

        public OrganizationActorMap FindForActor( IActorRef actor ) => Maps.FirstOrDefault( m => m.Actor?.Equals( actor ) == true );
    }
}