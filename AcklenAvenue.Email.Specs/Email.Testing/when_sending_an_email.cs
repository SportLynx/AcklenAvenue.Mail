using System.Threading;
using System.Threading.Tasks;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_sending_an_email
    {
        const string RecipientEmailAddress = "something@email.com";
        const string EmailBody = "email body";
        const string Subject = "Account Verification";

        static IEmailSender _emailSender;
        static IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        static IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        static TestModel _model;
        static IEmailClient _emailClient;
        static IEmailSubjectRenderer _emailSubjectRenderer;

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
                Mock.Get(_emailClient)
                    .Setup(x => x.SendAsync(Moq.It.IsAny<EmailMessage>(), Moq.It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(0));
                Mock.Get(_emailSubjectRenderer).Setup(x => x.Render(_model)).Returns(Subject);
            };

        Because of =
            () => _emailSender.SendAsync(RecipientEmailAddress, _model).GetAwaiter().GetResult();

        It should_send_the_expected_email_message =
            () => Mock.Get(_emailClient).Verify(x => x.SendAsync(Moq.It.Is<EmailMessage>(message =>
                message.To.Count == 1
                && message.To[0].Address == RecipientEmailAddress
                && message.To[0].DisplayName == null
                && message.Subject == Subject
                && message.HtmlBody == EmailBody
                && message.PlainTextBody == null
                && message.From == null
                && message.ReplyTo == null
                && message.Cc.Count == 0
                && message.Bcc.Count == 0
                && message.Attachments.Count == 0), Moq.It.IsAny<CancellationToken>()));
    }
}
