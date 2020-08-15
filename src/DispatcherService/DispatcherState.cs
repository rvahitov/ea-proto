using System.Collections.Immutable;
using System.Linq;

namespace DispatcherService
{
    public sealed class DispatcherState
    {
        private readonly ImmutableList<OrganizationActorMap> _maps;

        public DispatcherState(ImmutableList<OrganizationActorMap> maps)
        {
            _maps = maps;
        }

        public DispatcherState()
        {
            _maps = ImmutableList<OrganizationActorMap>.Empty;
        }

        public bool Contains(OrganizationActorMap map) => _maps.Contains(map);

        public DispatcherState Add(OrganizationActorMap map) =>
            new DispatcherState(_maps.Add(map));

        public DispatcherState Remove(OrganizationActorMap map) =>
            new DispatcherState(_maps.Remove(map));

        public OrganizationActorMap[] FindForOrganization(string organization) =>
            _maps.Where(m => m.Organization == organization).ToArray();
    }
}