namespace AcklenAvenue.Email
{
    public interface IEmailHtmlTemplateProvider
    {
        string GetTemplateFor<T>(T model);
    }
}