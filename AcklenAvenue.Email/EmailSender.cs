using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace AcklenAvenue.Email
{
    public class EmailSender : IEmailSender
    {
        const int MaxSendAttempts = 3;
        const string TimeoutMessageFragment = "timed out";
        static readonly TimeSpan BaseRetryDelay = TimeSpan.FromSeconds(1);

        readonly IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        readonly IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        readonly IEmailSubjectRenderer _emailSubjectRenderer;
        readonly IEmailClient _emailClient;

        public EmailSender(IEmailBodyHtmlRenderer emailBodyHtmlRenderer,
            IEmailBodyPlainTextRenderer emailBodyPlainTextRenderer,
            IEmailSubjectRenderer emailSubjectRenderer,
            IEmailClient emailClient)
        {
            _emailBodyHtmlRenderer = emailBodyHtmlRenderer;
            _emailBodyPlainTextRenderer = emailBodyPlainTextRenderer;
            _emailSubjectRenderer = emailSubjectRenderer;
            _emailClient = emailClient;
        }

        public Task SendAsync<T>(string emailAddress, T model,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var message = new EmailMessage
            {
                To = ParseAddressList(emailAddress)
            };

            return SendAsync(message, model, cancellationToken);
        }

        public Task SendAsync<T>(EmailMessage message, T model,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var renderedMessage = CreateMessage(message, model);
            return SendWithRetriesAsync(() => _emailClient.SendAsync(renderedMessage, cancellationToken),
                cancellationToken);
        }

        async Task SendWithRetriesAsync(Func<Task> sendAsync, CancellationToken cancellationToken)
        {
            for (var attempt = 1; attempt <= MaxSendAttempts; attempt++)
            {
                try
                {
                    await sendAsync().ConfigureAwait(false);
                    return;
                }
                catch (Exception exception) when (attempt < MaxSendAttempts && IsTimeoutFailure(exception, cancellationToken))
                {
                    await Task.Delay(GetRetryDelay(attempt), cancellationToken).ConfigureAwait(false);
                }
            }
        }

        static bool IsTimeoutFailure(Exception exception, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            if (exception is TimeoutException)
                return true;

            if (exception is TaskCanceledException)
                return true;

            var smtpException = exception as SmtpException;
            return smtpException != null
                && smtpException.StatusCode == SmtpStatusCode.GeneralFailure
                && !string.IsNullOrWhiteSpace(smtpException.Message)
                && smtpException.Message.IndexOf(TimeoutMessageFragment, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        EmailMessage CreateMessage<T>(EmailMessage message, T model)
        {
            return new EmailMessage
            {
                From = CopyAddress(message.From),
                ReplyTo = CopyAddress(message.ReplyTo),
                To = CopyAddresses(message.To),
                Cc = CopyAddresses(message.Cc),
                Bcc = CopyAddresses(message.Bcc),
                Subject = _emailSubjectRenderer.Render(model),
                HtmlBody = _emailBodyHtmlRenderer.Render(model),
                PlainTextBody = _emailBodyPlainTextRenderer.Render(model),
                Attachments = CopyAttachments(message.Attachments)
            };
        }

        static EmailAddress CopyAddress(EmailAddress address)
        {
            if (address == null)
                return null;

            return new EmailAddress
            {
                Address = address.Address,
                DisplayName = address.DisplayName
            };
        }

        static IReadOnlyList<EmailAddress> CopyAddresses(IReadOnlyList<EmailAddress> addresses)
        {
            if (addresses == null || addresses.Count == 0)
                return new EmailAddress[0];

            var copiedAddresses = new List<EmailAddress>(addresses.Count);
            foreach (var address in addresses)
                copiedAddresses.Add(CopyAddress(address));

            return copiedAddresses;
        }

        static IReadOnlyList<EmailAddress> ParseAddressList(string addresses)
        {
            if (string.IsNullOrWhiteSpace(addresses))
                return new EmailAddress[0];

            var collection = new MailAddressCollection();
            collection.Add(addresses.Replace(';', ','));

            var parsedAddresses = new List<EmailAddress>(collection.Count);

            foreach (MailAddress address in collection)
            {
                parsedAddresses.Add(new EmailAddress
                {
                    Address = address.Address,
                    DisplayName = string.IsNullOrWhiteSpace(address.DisplayName) ? null : address.DisplayName
                });
            }

            return parsedAddresses;
        }

        static IReadOnlyList<EmailAttachment> CopyAttachments(IReadOnlyList<EmailAttachment> attachments)
        {
            if (attachments == null || attachments.Count == 0)
                return new EmailAttachment[0];

            var mappedAttachments = new List<EmailAttachment>(attachments.Count);

            foreach (var attachment in attachments)
            {
                mappedAttachments.Add(new EmailAttachment
                {
                    FileName = attachment?.FileName,
                    ContentType = attachment?.ContentType,
                    Content = attachment?.Content == null ? null : (byte[])attachment.Content.Clone()
                });
            }

            return mappedAttachments;
        }

        static TimeSpan GetRetryDelay(int attempt)
        {
            return TimeSpan.FromMilliseconds(BaseRetryDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
        }
    }
}
