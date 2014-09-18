namespace AcklenAvenue.Email.Email
{
    public class EmailSender : IEmailSender
    {
        readonly IEmailBodyRenderer _emailBodyRenderer;
        readonly IEmailSubjectRenderer _emailSubjectRenderer;
        readonly ISmtpClient _smtpClient;

        public EmailSender(IEmailBodyRenderer emailBodyRenderer, IEmailSubjectRenderer emailSubjectRenderer, ISmtpClient smtpClient)
        {
            _emailBodyRenderer = emailBodyRenderer;
            _emailSubjectRenderer = emailSubjectRenderer;
            _smtpClient = smtpClient;
        }

        public void Send<T>(string emailAddress, T model)
        {
            string subject = _emailSubjectRenderer.Render(model);
            string body = _emailBodyRenderer.Render(model);
            _smtpClient.Send(emailAddress, subject, body);
        }
    }
}