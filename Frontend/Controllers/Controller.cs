using NLog;
using System.Web.Http;

namespace Frontend.Controllers
{
    public class Controller : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string GetBaseUri()
        {
            var completeUri = Request.RequestUri.AbsoluteUri;
            var baseUri = completeUri.Replace(Request.RequestUri.AbsolutePath, "");
            return $"{baseUri}/";
        }

        public System.Uri CreateCompleteUri(string relativeUrl)
        {
            var completeUrl = GetBaseUri() + relativeUrl;
            return new System.Uri(completeUrl);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
    }
}