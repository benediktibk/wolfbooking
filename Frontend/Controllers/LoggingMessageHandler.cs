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
            logger.Debug($"{request.Method} {request.RequestUri}");
            if (request.Headers.Authorization != null)
                logger.Debug($"{request.Headers.Authorization.Scheme} {request.Headers.Authorization.Parameter}");

            var response = await base.SendAsync(request, cancellationToken);
            var responseContent = response.Content?.ReadAsStringAsync().Result;

            logger.Debug($"{(int)response.StatusCode} - {response.StatusCode}");
            if (!string.IsNullOrEmpty(responseContent))
                logger.Debug($"\n{responseContent}");

            return response;
        }

        private void LogRequestAndResponse(HttpRequestMessage request, HttpResponseMessage response)
        {

        }
    }
}