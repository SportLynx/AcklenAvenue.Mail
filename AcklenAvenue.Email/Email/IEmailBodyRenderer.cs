namespace AcklenAvenue.Email.Email
{
    public interface IEmailBodyRenderer
    {
        string Render<T>(T model);
    }
}