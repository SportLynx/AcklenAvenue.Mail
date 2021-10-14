using System;

namespace AcklenAvenue.Email
{
    public interface IEmailBodyPlainTextTemplate
    {
        Type ForType { get; }
        string BodyPlainTextTemplate { get; }
    }
}