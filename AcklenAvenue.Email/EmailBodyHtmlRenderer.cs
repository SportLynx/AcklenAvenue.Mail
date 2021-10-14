namespace AcklenAvenue.Email
{
    public class EmailBodyHtmlRenderer : IEmailBodyHtmlRenderer
    {
        readonly IEmailHtmlTemplateProvider _emailHtmlTemplateProvider;
        readonly IViewEngine _viewEngine;

        public EmailBodyHtmlRenderer(IEmailHtmlTemplateProvider emailHtmlTemplateProvider, IViewEngine viewEngine)
        {
            _emailHtmlTemplateProvider = emailHtmlTemplateProvider;
            _viewEngine = viewEngine;
        }

        public string Render<T>(T model)
        {
            return _viewEngine.Render(model, _emailHtmlTemplateProvider.GetTemplateFor(model));
        }
    }
}