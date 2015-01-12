namespace AcklenAvenue.Email
{
    public interface ISmtpClient
    {
        void Send(string fromAddress, string subject, string body);
        void Send(string replyToAddress, string replyToName, string fromAddress, string fromName, string recipientList, string subject, string body);
    }
}