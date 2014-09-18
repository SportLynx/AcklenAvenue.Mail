using System.Collections.Generic;
using AcklenAvenue.Email.Email;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_providing_a_template_from_a_model
    {
        const string Template = "template";
        static IEmailTemplateProvider _emailTemplateProvider;
        static TestModel _model;
        static string _result;

        Establish context =
            () =>
            {
                var template = Mock.Of<IEmailBodyTemplate>();
                _emailTemplateProvider = new EmailTemplateProvider(new List<IEmailBodyTemplate>
                                                                   {
                                                                       template
                                                                   });

                Mock.Get(template).Setup(x => x.ForType).Returns(typeof(TestModel));

                Mock.Get(template).Setup(x => x.BodyTemplate).Returns(Template);

                _model = new TestModel();
            };

        Because of =
            () => _result = _emailTemplateProvider.GetTemplateFor(_model);

        It should_return_the_expected_template =
            () => _result.ShouldEqual(Template);
    }
}