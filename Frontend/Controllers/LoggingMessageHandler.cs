using NLog;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend.Controllers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            logger.Debug($"{request.RequestUri} - {request.Method}");
            var response = await base.SendAsync(request, cancellationToken);
            logger.Debug($"{(int)response.StatusCode} - {response.StatusCode}");
            return response;
        }
    }
}