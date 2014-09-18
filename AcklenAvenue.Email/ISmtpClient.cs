namespace AcklenAvenue.Email
{
    public interface ISmtpClient
    {
        void Send(string emailAddress, string subject, string body);
    }
}