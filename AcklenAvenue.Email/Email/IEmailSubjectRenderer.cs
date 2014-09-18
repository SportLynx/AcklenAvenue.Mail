namespace AcklenAvenue.Email.Email
{
    public interface IEmailSubjectRenderer
    {
        string Render<T>(T model);
    }
}