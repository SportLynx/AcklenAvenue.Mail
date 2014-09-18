using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AcklenAvenue.Email
{
    public class AcklenSmtpClient : ISmtpClient
    {
        public string @from { get; private set; }

        #region ISmtpClient Members

        public void Send(string emailAddress, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                
                if (string.IsNullOrEmpty(from))
                    throw new Exception("You need to define the from.");

                var message = new MailMessage(from, emailAddress, subject, body) { IsBodyHtml = true };
                client.Send(message);
            }
        }

        private AcklenSmtpClient(string @from)
        {
            this.@from = @from;
        }

        public static AcklenSmtpClient From(string from)
        {
            return new AcklenSmtpClient(from);
        }

        #endregion
    }
}
