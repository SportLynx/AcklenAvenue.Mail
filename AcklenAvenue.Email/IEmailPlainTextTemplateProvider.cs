namespace AcklenAvenue.Email
{
    public interface IEmailPlainTextTemplateProvider
    {
        string GetTemplateFor<T>(T model);
    }
}