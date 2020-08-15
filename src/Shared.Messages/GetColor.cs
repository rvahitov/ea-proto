using System;

namespace Shared.Messages
{
    public sealed class GetColor
    {
        public GetColor(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId)) throw new ArgumentException("Value cannot be null or empty.", nameof(organizationId));
            OrganizationId = organizationId;
        }

        public string OrganizationId { get; }
    }

    public sealed class GetColorResponse
    {
        public GetColorResponse(string color, bool isSuccess, string[] errors)
        {
            Color = color;
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public string Color { get; }
        public bool IsSuccess { get; }
        public string[] Errors { get; }
    }
}