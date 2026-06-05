using System.Threading;
using System.Threading.Tasks;

namespace AcklenAvenue.Email
{
    public interface IEmailSender
    {
        Task SendAsync<T>(string emailAddress, T emailModel, CancellationToken cancellationToken = default(CancellationToken));
        Task SendAsync<T>(EmailMessage message, T emailModel, CancellationToken cancellationToken = default(CancellationToken));
    }
}
