using System.Collections.Generic;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_providing_a_template_from_a_model
    {
        const string Template = "template";
        static IEmailHtmlTemplateProvider _emailHtmlTemplateProvider;
        static TestModel _model;
        static string _result;

        Establish context =
            () =>
            {
                var template = Mock.Of<IEmailBodyHtmlTemplate>();
                _emailHtmlTemplateProvider = new EmailHtmlTemplateProvider(new List<IEmailBodyHtmlTemplate>
                                                                   {
                                                                       template
                                                                   });

                Mock.Get(template).Setup(x => x.ForType).Returns(typeof(TestModel));

                Mock.Get(template).Setup(x => x.BodyHtmlTemplate).Returns(Template);

                _model = new TestModel();
            };

        Because of =
            () => _result = _emailHtmlTemplateProvider.GetTemplateFor(_model);

        It should_return_the_expected_template =
            () => _result.ShouldEqual(Template);
    }
}