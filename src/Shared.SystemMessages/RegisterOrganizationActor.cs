using Akka.Actor;

namespace Shared.SystemMessages
{
    public sealed class RegisterOrganizationActor
    {
        public RegisterOrganizationActor(string organizationId, IActorRef organizationActor)
        {
            OrganizationId = organizationId;
            OrganizationActor = organizationActor;
        }

        public string OrganizationId { get; }
        public IActorRef OrganizationActor { get; }
    }

    public sealed class UnregisterOrganizationActor
    {
        public UnregisterOrganizationActor(string organizationId, IActorRef organizationActor)
        {
            OrganizationId = organizationId;
            OrganizationActor = organizationActor;
        }

        public string OrganizationId { get; }
        public IActorRef OrganizationActor { get; }
    }
}