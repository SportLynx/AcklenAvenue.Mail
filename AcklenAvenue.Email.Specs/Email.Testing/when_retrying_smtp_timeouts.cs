using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_retrying_a_timed_out_send
    {
        const string EmailAddress = "something@email.com";
        const string EmailBody = "email body";
        const string Subject = "Account Verification";

        static IEmailSender _emailSender;
        static IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        static IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        static IEmailSubjectRenderer _emailSubjectRenderer;
        static TestModel _model;
        static IEmailClient _emailClient;
        static Exception _exception;
        static int _sendAttempts;

        Establish context =
            () =>
            {
                _emailBodyHtmlRenderer = Mock.Of<IEmailBodyHtmlRenderer>();
                _emailBodyPlainTextRenderer = Mock.Of<IEmailBodyPlainTextRenderer>();
                _emailClient = Mock.Of<IEmailClient>();
                _emailSubjectRenderer = Mock.Of<IEmailSubjectRenderer>();
                _emailSender = new EmailSender(_emailBodyHtmlRenderer, _emailBodyPlainTextRenderer,
                    _emailSubjectRenderer, _emailClient);

                _model = new TestModel();

                Mock.Get(_emailBodyHtmlRenderer).Setup(x => x.Render(_model)).Returns(EmailBody);
                Mock.Get(_emailSubjectRenderer).Setup(x => x.Render(_model)).Returns(Subject);
                Mock.Get(_emailClient)
                    .Setup(x => x.SendAsync(Moq.It.IsAny<EmailMessage>(), Moq.It.IsAny<CancellationToken>()))
                    .Callback(() =>
                    {
                        _sendAttempts++;

                        if (_sendAttempts == 1)
                            throw new SmtpException(SmtpStatusCode.GeneralFailure, "The operation has timed out");
                    })
                    .Returns(Task.FromResult(0));
            };

        Because of =
            () =>
            {
                try
                {
                    _emailSender.SendAsync(EmailAddress, _model).GetAwaiter().GetResult();
                }
                catch (Exception exception)
                {
                    _exception = exception;
                }
            };

        It should_retry_the_send =
            () => _sendAttempts.ShouldEqual(2);

        It should_not_bubble_the_timeout =
            () => (_exception == null).ShouldEqual(true);
    }

    public class when_retrying_a_timed_out_send_with_attachments
    {
        const string ReplyToAddress = "replyto@email.com";
        const string ReplyToName = "Reply To";
        const string FromAddress = "from@email.com";
        const string FromName = "From";
        const string RecipientToList = "to@email.com";
        const string RecipientCcList = "cc@email.com";
        const string RecipientBccList = "bcc@email.com";
        const string HtmlBody = "<p>html body</p>";
        const string PlainTextBody = "plain text body";
        const string Subject = "Account Verification";
        const string AttachmentName = "attachment.txt";
        const string AttachmentContentType = "application/octet-stream";

        static readonly byte[] AttachmentContent = { 1, 2, 3 };
        static IEmailSender _emailSender;
        static IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        static IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        static IEmailSubjectRenderer _emailSubjectRenderer;
        static TestModel _model;
        static List<EmailMessage> _messages;
        static IEmailClient _emailClient;
        static Exception _exception;
        static int _sendAttempts;
        static EmailMessage _messageTemplate;

        Establish context =
            () =>
            {
                _emailBodyHtmlRenderer = Mock.Of<IEmailBodyHtmlRenderer>();
                _emailBodyPlainTextRenderer = Mock.Of<IEmailBodyPlainTextRenderer>();
                _emailClient = Mock.Of<IEmailClient>();
                _emailSubjectRenderer = Mock.Of<IEmailSubjectRenderer>();
                _messages = new List<EmailMessage>();
                _emailSender = new EmailSender(_emailBodyHtmlRenderer, _emailBodyPlainTextRenderer,
                    _emailSubjectRenderer, _emailClient);

                _messageTemplate = new EmailMessage
                {
                    ReplyTo = new EmailAddress { Address = ReplyToAddress, DisplayName = ReplyToName },
                    From = new EmailAddress { Address = FromAddress, DisplayName = FromName },
                    To = new[] { new EmailAddress { Address = RecipientToList } },
                    Cc = new[] { new EmailAddress { Address = RecipientCcList } },
                    Bcc = new[] { new EmailAddress { Address = RecipientBccList } },
                    Attachments = new[]
                    {
                        new EmailAttachment
                        {
                            FileName = AttachmentName,
                            ContentType = AttachmentContentType,
                            Content = AttachmentContent
                        }
                    }
                };

                _model = new TestModel();

                Mock.Get(_emailBodyHtmlRenderer).Setup(x => x.Render(_model)).Returns(HtmlBody);
                Mock.Get(_emailBodyPlainTextRenderer).Setup(x => x.Render(_model)).Returns(PlainTextBody);
                Mock.Get(_emailSubjectRenderer).Setup(x => x.Render(_model)).Returns(Subject);
                Mock.Get(_emailClient)
                    .Setup(x => x.SendAsync(Moq.It.IsAny<EmailMessage>(), Moq.It.IsAny<CancellationToken>()))
                    .Callback<EmailMessage, CancellationToken>((message, cancellationToken) =>
                    {
                        _sendAttempts++;
                        _messages.Add(message);

                        if (_sendAttempts == 1)
                            throw new SmtpException(SmtpStatusCode.GeneralFailure, "The operation has timed out");
                    })
                    .Returns(Task.FromResult(0));
            };

        Because of =
            () =>
            {
                try
                {
                    _emailSender.SendAsync(_messageTemplate, _model).GetAwaiter().GetResult();
                }
                catch (Exception exception)
                {
                    _exception = exception;
                }
            };

        It should_retry_the_send =
            () => _sendAttempts.ShouldEqual(2);

        It should_preserve_the_attachment_content_on_each_attempt =
            () => _messages.All(message =>
                message.Attachments.Count == 1
                && message.Attachments[0].FileName == AttachmentName
                && message.Attachments[0].ContentType == AttachmentContentType
                && message.Attachments[0].Content.SequenceEqual(AttachmentContent)).ShouldEqual(true);

        It should_map_the_message_properties =
            () => (_messages[0].ReplyTo.Address == ReplyToAddress
                && _messages[0].ReplyTo.DisplayName == ReplyToName
                && _messages[0].From.Address == FromAddress
                && _messages[0].From.DisplayName == FromName
                && _messages[0].To.Count == 1
                && _messages[0].To[0].Address == RecipientToList
                && _messages[0].Cc.Count == 1
                && _messages[0].Cc[0].Address == RecipientCcList
                && _messages[0].Bcc.Count == 1
                && _messages[0].Bcc[0].Address == RecipientBccList
                && _messages[0].Subject == Subject
                && _messages[0].HtmlBody == HtmlBody
                && _messages[0].PlainTextBody == PlainTextBody).ShouldEqual(true);

        It should_not_mutate_the_original_message =
            () => (_messageTemplate.Subject == null
                && _messageTemplate.HtmlBody == null
                && _messageTemplate.PlainTextBody == null).ShouldEqual(true);

        It should_not_bubble_the_timeout =
            () => (_exception == null).ShouldEqual(true);
    }

    public class when_email_send_fails_with_a_non_timeout_exception
    {
        const string EmailAddress = "something@email.com";
        const string EmailBody = "email body";
        const string Subject = "Account Verification";

        static IEmailSender _emailSender;
        static IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        static IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        static IEmailSubjectRenderer _emailSubjectRenderer;
        static TestModel _model;
        static IEmailClient _emailClient;
        static Exception _exception;
        static int _sendAttempts;

        Establish context =
            () =>
            {
                _emailBodyHtmlRenderer = Mock.Of<IEmailBodyHtmlRenderer>();
                _emailBodyPlainTextRenderer = Mock.Of<IEmailBodyPlainTextRenderer>();
                _emailClient = Mock.Of<IEmailClient>();
                _emailSubjectRenderer = Mock.Of<IEmailSubjectRenderer>();
                _emailSender = new EmailSender(_emailBodyHtmlRenderer, _emailBodyPlainTextRenderer,
                    _emailSubjectRenderer, _emailClient);

                _model = new TestModel();

                Mock.Get(_emailBodyHtmlRenderer).Setup(x => x.Render(_model)).Returns(EmailBody);
                Mock.Get(_emailSubjectRenderer).Setup(x => x.Render(_model)).Returns(Subject);
                Mock.Get(_emailClient)
                    .Setup(x => x.SendAsync(Moq.It.IsAny<EmailMessage>(), Moq.It.IsAny<CancellationToken>()))
                    .Callback(() =>
                    {
                        _sendAttempts++;
                        throw new SmtpException(SmtpStatusCode.ClientNotPermitted, "Authentication failed");
                    })
                    .Returns(Task.FromResult(0));
            };

        Because of =
            () =>
            {
                try
                {
                    _emailSender.SendAsync(EmailAddress, _model).GetAwaiter().GetResult();
                }
                catch (Exception exception)
                {
                    _exception = exception;
                }
            };

        It should_not_retry_the_send =
            () => _sendAttempts.ShouldEqual(1);

        It should_bubble_the_exception =
            () => (_exception is SmtpException).ShouldEqual(true);
    }
}
