using System;

namespace AcklenAvenue.Email
{
    public interface IEmailSubjectTemplate
    {
        Type ForType { get; }
        string SubjectTemplate { get; }
    }
}