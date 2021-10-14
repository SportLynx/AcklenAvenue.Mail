namespace AcklenAvenue.Email
{
    public class EmailBodyPlainTextRenderer : IEmailBodyPlainTextRenderer
    {
        readonly IEmailPlainTextTemplateProvider _emailPlainTextTemplateProvider;
        readonly IViewEngine _viewEngine;

        public EmailBodyPlainTextRenderer(IEmailPlainTextTemplateProvider emailPlainTextTemplateProvider, IViewEngine viewEngine)
        {
            _emailPlainTextTemplateProvider = emailPlainTextTemplateProvider;
            _viewEngine = viewEngine;
        }

        public string Render<T>(T model)
        {
            return _viewEngine.Render(model, _emailPlainTextTemplateProvider.GetTemplateFor(model));
        }
    }
}