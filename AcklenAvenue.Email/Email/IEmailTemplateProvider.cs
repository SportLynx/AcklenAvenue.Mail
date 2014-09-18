namespace AcklenAvenue.Email.Email
{
    public interface IEmailTemplateProvider
    {
        string GetTemplateFor<T>(T model);
    }
}