using System.Collections.Generic;

namespace AcklenAvenue.Email
{
    public sealed class EmailMessage
    {
        public EmailAddress From { get; set; }
        public EmailAddress ReplyTo { get; set; }

        public IReadOnlyList<EmailAddress> To { get; set; } = new EmailAddress[0];
        public IReadOnlyList<EmailAddress> Cc { get; set; } = new EmailAddress[0];
        public IReadOnlyList<EmailAddress> Bcc { get; set; } = new EmailAddress[0];

        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainTextBody { get; set; }

        public IReadOnlyList<EmailAttachment> Attachments { get; set; } = new EmailAttachment[0];
    }
}
