using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_sending_an_email
    {
        const string EmailAddress = "something@email.com";
        const string EmailBody = "email body";
        const string Subject = "Account Verification";
        static IEmailSender _emailSender;
        static IEmailBodyRenderer _emailBodyRenderer;
        static TestModel _model;
        static ISmtpClient _smtpClient;
        static IEmailSubjectRenderer _emailSubjectRenderer;

        Establish context =
            () =>
            {
                _emailBodyRenderer = Mock.Of<IEmailBodyRenderer>();
                _smtpClient = Mock.Of<ISmtpClient>();
                _emailSubjectRenderer = Mock.Of<IEmailSubjectRenderer>();
                _emailSender = new EmailSender(_emailBodyRenderer, _emailSubjectRenderer, _smtpClient);

                _model = new TestModel();

                Mock.Get(_emailBodyRenderer).Setup(x => x.Render(_model)).Returns(EmailBody);

                Mock.Get(_emailSubjectRenderer).Setup(x => x.Render(_model)).Returns(Subject);
            };

        Because of =
            () => _emailSender.Send(EmailAddress, _model);

        It should_send_the_expected_email_body =
            () => Mock.Get(_smtpClient).Verify(x => x.Send(EmailAddress, Subject, EmailBody));
    }
}