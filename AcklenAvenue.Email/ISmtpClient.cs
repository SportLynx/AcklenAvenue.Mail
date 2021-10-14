using System.Collections.Generic;
using System.IO;

namespace AcklenAvenue.Email
{
    public interface ISmtpClient
    {
        //SUPER SIMPLE
        void Send(string recipientList, string subject, string body);

        //The Kitchen Sink
        void Send(string replyToAddress, string replyToName, string fromAddress, string fromName,
            string recipientToList, string recipientCcList, string recipientBccList, string subject, string htmlBody,
            string plainTextBody, Dictionary<string, MemoryStream> attachments = null,
            Dictionary<string, string> headers = null, string smtpUsername = null, string smtpPassword = null);
    }
}