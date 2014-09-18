using System;

namespace AcklenAvenue.Email.Email
{
    public interface IEmailBodyTemplate
    {
        Type ForType { get; }
        string BodyTemplate { get; }
    }
}