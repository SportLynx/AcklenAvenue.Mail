namespace AcklenAvenue.Email
{
    public interface IEmailSubjectRenderer
    {
        string Render<T>(T model);
    }
}