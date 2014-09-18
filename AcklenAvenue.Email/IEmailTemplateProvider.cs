namespace AcklenAvenue.Email
{
    public interface IEmailTemplateProvider
    {
        string GetTemplateFor<T>(T model);
    }
}