using System;
using System.Collections.Generic;
using System.Linq;

namespace AcklenAvenue.Email
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        readonly IEnumerable<IEmailBodyTemplate> _templates;

        public EmailTemplateProvider(IEnumerable<IEmailBodyTemplate> templates)
        {
            _templates = templates;
        }

        #region ITemplateProvider Members

        public string GetTemplateFor<T>(T model)
        {
            IEmailBodyTemplate emailBodyTemplate = _templates.FirstOrDefault(x => x.ForType == model.GetType());

            if (emailBodyTemplate == null)
                throw new Exception(string.Format("No template available for model type '{0}'.", model.GetType()));

            return emailBodyTemplate.BodyTemplate;
        }

        #endregion
    }
}