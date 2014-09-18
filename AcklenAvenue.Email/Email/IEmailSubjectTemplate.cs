using System;

namespace AcklenAvenue.Email.Email
{
    public interface IEmailSubjectTemplate
    {
        Type ForType { get; }
        string SubjectTemplate { get; }
    }
}