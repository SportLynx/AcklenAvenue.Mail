namespace AcklenAvenue.Email
{
    public interface IEmailBodyPlainTextRenderer
    {
        string Render<T>(T model);
    }
}