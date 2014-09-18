using System.Collections.Generic;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace AcklenAvenue.Email.Specs.Email.Testing
{
    public class when_getting_a_subject_for_a_model
    {
        const string Subject = "subject";
        static IEmailSubjectRenderer _subjectRenderer;
        static TestModel _model;
        static string _result;
        static IEmailSubjectTemplate _subjectTemplate;

        Establish context =
            () =>
            {
                _subjectTemplate = Mock.Of<IEmailSubjectTemplate>();
                _subjectRenderer = new EmailSubjectRenderer(new List<IEmailSubjectTemplate> { _subjectTemplate });

                _model = new TestModel();

                Mock.Get(_subjectTemplate).Setup(x => x.ForType).Returns(_model.GetType());
                Mock.Get(_subjectTemplate).Setup(x => x.SubjectTemplate).Returns(Subject);
            };

        Because of =
            () => _result = _subjectRenderer.Render(_model);

        It should_return_the_expected_subject =
            () => _result.ShouldEqual(Subject);
    }
}