using System.Collections.Generic;
using System.IO;

namespace AcklenAvenue.Email
{
    public interface IEmailSender
    {
        void Send<T>(string emailAddress, T emailModel);
        void Send<T>(string replyToAddress, string replyToName, string fromAddress, string fromName, string recipientToList, string recipientCcList, string recipientBccList, T model, Dictionary<string, MemoryStream> attachments = null, Dictionary<string, string> headers = null, string smtpUsername = null, string smtpPassword = null);
    }
}