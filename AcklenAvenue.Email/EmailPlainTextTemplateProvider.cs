using System;
using System.Collections.Generic;
using System.Linq;

namespace AcklenAvenue.Email
{
    public class EmailPlainTextTemplateProvider : IEmailPlainTextTemplateProvider
    {
        readonly IEnumerable<IEmailBodyPlainTextTemplate> _templates;

        public EmailPlainTextTemplateProvider(IEnumerable<IEmailBodyPlainTextTemplate> templates)
        {
            _templates = templates;
        }

        #region ITemplateProvider Members

        public string GetTemplateFor<T>(T model)
        {
            IEmailBodyPlainTextTemplate emailBodyPlainTextTemplate = _templates.FirstOrDefault(x => x.ForType == model.GetType());

            if (emailBodyPlainTextTemplate == null)
                throw new Exception(string.Format("No template available for model type '{0}'.", model.GetType()));

            return emailBodyPlainTextTemplate.BodyPlainTextTemplate;
        }

        #endregion
    }
}