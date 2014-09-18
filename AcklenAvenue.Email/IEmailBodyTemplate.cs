using System;

namespace AcklenAvenue.Email
{
    public interface IEmailBodyTemplate
    {
        Type ForType { get; }
        string BodyTemplate { get; }
    }
}