using System.Threading;
using System.Threading.Tasks;

namespace AcklenAvenue.Email
{
    public interface IEmailClient
    {
        Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
