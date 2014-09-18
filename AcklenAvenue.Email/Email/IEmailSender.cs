namespace AcklenAvenue.Email.Email
{
    public interface IEmailSender
    {
        void Send<T>(string emailAddress, T emailModel);
    }
}