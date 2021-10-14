using System;
using System.Collections.Generic;
using System.Linq;

namespace AcklenAvenue.Email
{
    public class EmailHtmlTemplateProvider : IEmailHtmlTemplateProvider
    {
        readonly IEnumerable<IEmailBodyHtmlTemplate> _templates;

        public EmailHtmlTemplateProvider(IEnumerable<IEmailBodyHtmlTemplate> templates)
        {
            _templates = templates;
        }

        #region ITemplateProvider Members

        public string GetTemplateFor<T>(T model)
        {
            IEmailBodyHtmlTemplate emailBodyHtmlTemplate = _templates.FirstOrDefault(x => x.ForType == model.GetType());

            if (emailBodyHtmlTemplate == null)
                throw new Exception(string.Format("No template available for model type '{0}'.", model.GetType()));

            return emailBodyHtmlTemplate.BodyHtmlTemplate;
        }

        #endregion
    }
}