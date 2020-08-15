using System;
using Akka.Actor;

namespace DispatcherService
{
    public sealed class OrganizationActorMap : IEquatable<OrganizationActorMap>
    {
        public OrganizationActorMap(string organization, IActorRef actor)
        {
            Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            Actor = actor ?? throw new ArgumentNullException(nameof(actor));
        }

        public string Organization { get; }
        public IActorRef Actor { get; }

        public bool Equals(OrganizationActorMap other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Organization == other.Organization && Actor.Equals(other.Actor);
        }

        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is OrganizationActorMap other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Organization, Actor);

        public static bool operator ==(OrganizationActorMap left, OrganizationActorMap right) => Equals(left, right);

        public static bool operator !=(OrganizationActorMap left, OrganizationActorMap right) => !Equals(left, right);
    }
}