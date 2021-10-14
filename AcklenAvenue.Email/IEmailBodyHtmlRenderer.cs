namespace AcklenAvenue.Email
{
    public interface IEmailBodyHtmlRenderer
    {
        string Render<T>(T model);
    }
}