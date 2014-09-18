namespace AcklenAvenue.Email.Email
{
    public interface ISmtpClient
    {
        void Send(string emailAddress, string subject, string body);
    }
}