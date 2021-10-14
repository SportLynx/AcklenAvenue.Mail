using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_rendering_the_body_of_an_email_from_a_model
    {
        const string Template = "template";
        const string RenderedHtml = "rendered html";
        static IEmailBodyHtmlRenderer _renderer;
        static TestModel _model;
        static string _result;
        static IEmailHtmlTemplateProvider _emailHtmlTemplateProvider;
        static IViewEngine _viewEngine;

        Establish context =
            () =>
            {
                _emailHtmlTemplateProvider = Mock.Of<IEmailHtmlTemplateProvider>();
                _viewEngine = Mock.Of<IViewEngine>();
                _renderer = new EmailBodyHtmlRenderer(_emailHtmlTemplateProvider, _viewEngine);

                _model = new TestModel { };

                Mock.Get(_emailHtmlTemplateProvider).Setup(x => x.GetTemplateFor(_model)).Returns(Template);

                Mock.Get(_viewEngine).Setup(x => x.Render(_model, Template)).Returns(RenderedHtml);
            };

        Because of =
            () => _result = _renderer.Render(_model);

        It should_return_the_expected_string =
            () => _result.ShouldEqual(RenderedHtml);
    }
}