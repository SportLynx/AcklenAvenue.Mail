using System;

namespace AcklenAvenue.Email
{
    public interface IEmailBodyHtmlTemplate
    {
        Type ForType { get; }
        string BodyHtmlTemplate { get; }
    }
}