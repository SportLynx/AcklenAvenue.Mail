namespace AcklenAvenue.Email
{
    public interface IViewEngine
    {
        string Render(object model, string formattedString);
    }
}