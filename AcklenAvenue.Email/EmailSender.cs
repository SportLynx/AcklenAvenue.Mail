using System.Collections.Generic;
using System.IO;

namespace AcklenAvenue.Email
{
    public class EmailSender : IEmailSender
    {
        readonly IEmailBodyHtmlRenderer _emailBodyHtmlRenderer;
        readonly IEmailBodyPlainTextRenderer _emailBodyPlainTextRenderer;
        readonly IEmailSubjectRenderer _emailSubjectRenderer;
        readonly ISmtpClient _smtpClient;

        public EmailSender(IEmailBodyHtmlRenderer emailBodyHtmlRenderer, IEmailBodyPlainTextRenderer emailBodyPlainTextRenderer, IEmailSubjectRenderer emailSubjectRenderer, ISmtpClient smtpClient)
        {
            _emailBodyHtmlRenderer = emailBodyHtmlRenderer;
            _emailBodyPlainTextRenderer = emailBodyPlainTextRenderer;
            _emailSubjectRenderer = emailSubjectRenderer;
            _smtpClient = smtpClient;
        }

        public void Send<T>(string emailAddress, T model)
        {
            var subject = _emailSubjectRenderer.Render(model);
            var body = _emailBodyHtmlRenderer.Render(model);
            _smtpClient.Send(emailAddress, subject, body);
        }

        public void Send<T>(string replyToAddress, string replyToName, string fromAddress, string fromName, string recipientToList, string recipientCcList, string recipientBccList, T model, Dictionary<string, MemoryStream> attachments, Dictionary<string, string> headers, string smtpUsername = null, string smtpPassword = null)
        {
            var subject = _emailSubjectRenderer.Render(model);
            var bodyHtml = _emailBodyHtmlRenderer.Render(model);
            var bodyPlainText = _emailBodyPlainTextRenderer.Render(model);
            _smtpClient.Send(replyToAddress, replyToName, fromAddress, fromName, recipientToList, recipientCcList, recipientBccList, subject, bodyHtml, bodyPlainText, attachments, headers, smtpUsername, smtpPassword);
        }
    }
}