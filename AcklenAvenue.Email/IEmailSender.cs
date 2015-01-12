namespace AcklenAvenue.Email
{
    public interface IEmailSender
    {
        void Send<T>(string emailAddress, T emailModel);
        void Send<T>(string replyToAddress, string replyToNamw, string fromAddress, string fromName, string recipientList, T emailModel);
    }
}