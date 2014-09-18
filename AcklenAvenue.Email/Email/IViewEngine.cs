namespace AcklenAvenue.Email.Email
{
    public interface IViewEngine
    {
        string Render(object model, string formattedString);
    }
}