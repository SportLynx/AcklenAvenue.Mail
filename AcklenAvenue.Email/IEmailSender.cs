namespace AcklenAvenue.Email
{
    public interface IEmailSender
    {
        void Send<T>(string emailAddress, T emailModel);
    }
}