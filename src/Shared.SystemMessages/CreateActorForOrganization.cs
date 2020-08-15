namespace Shared.SystemMessages
{
    public sealed class CreateActorForOrganization
    {
        public CreateActorForOrganization(string organizationId, string color)
        {
            OrganizationId = organizationId;
            Color = color;
        }

        public string OrganizationId { get; }
        public string Color { get; }
    }

    public sealed class CreateActorForOrganizationResult
    {
        public CreateActorForOrganizationResult(bool isSuccess, string[] errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public bool IsSuccess { get; }
        public string[] Errors { get; }
    }
}