namespace AcklenAvenue.Email
{
    public class EmailBodyRenderer : IEmailBodyRenderer
    {
        readonly IEmailTemplateProvider _emailTemplateProvider;
        readonly IViewEngine _viewEngine;

        public EmailBodyRenderer(IEmailTemplateProvider emailTemplateProvider, IViewEngine viewEngine)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _viewEngine = viewEngine;
        }

        public string Render<T>(T model)
        {
            return _viewEngine.Render(model, _emailTemplateProvider.GetTemplateFor(model));
        }
    }
}