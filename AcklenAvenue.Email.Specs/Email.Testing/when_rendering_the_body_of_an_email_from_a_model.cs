using AcklenAvenue.Email.Email;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_rendering_the_body_of_an_email_from_a_model
    {
        const string Template = "template";
        const string RenderedHtml = "rendered html";
        static IEmailBodyRenderer _renderer;
        static TestModel _model;
        static string _result;
        static IEmailTemplateProvider _emailTemplateProvider;
        static IViewEngine _viewEngine;

        Establish context =
            () =>
            {
                _emailTemplateProvider = Mock.Of<IEmailTemplateProvider>();
                _viewEngine = Mock.Of<IViewEngine>();
                _renderer = new EmailBodyRenderer(_emailTemplateProvider, _viewEngine);

                _model = new TestModel { };

                Mock.Get(_emailTemplateProvider).Setup(x => x.GetTemplateFor(_model)).Returns(Template);

                Mock.Get(_viewEngine).Setup(x => x.Render(_model, Template)).Returns(RenderedHtml);
            };

        Because of =
            () => _result = _renderer.Render(_model);

        It should_return_the_expected_string =
            () => _result.ShouldEqual(RenderedHtml);
    }
}