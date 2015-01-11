using System;
using System.Collections.Generic;
using System.Linq;

namespace AcklenAvenue.Email
{
    public class EmailSubjectRenderer : IEmailSubjectRenderer
    {
        readonly IEnumerable<IEmailSubjectTemplate> _emailSubjects;
        readonly IViewEngine _viewEngine;

        public EmailSubjectRenderer(IEnumerable<IEmailSubjectTemplate> emailSubjects, IViewEngine viewEngine)
        {
            _emailSubjects = emailSubjects;
            _viewEngine = viewEngine;
        }

        public string Render<T>(T model)
        {
            IEmailSubjectTemplate subjectTemplate = _emailSubjects.FirstOrDefault(x => x.ForType == model.GetType());

            if (subjectTemplate == null)
                throw new Exception(string.Format("No email subject exists for model type '{0}'.", model.GetType()));

            var subject = _viewEngine.Render(model, subjectTemplate.SubjectTemplate);

            return subject;
        }
    }
}