namespace AcklenAvenue.Email
{
    public interface IEmailBodyRenderer
    {
        string Render<T>(T model);
    }
}