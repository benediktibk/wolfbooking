using System.Web;

namespace Facade.Controllers
{
    public static class UrlHelper
    {
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            return $"{request.Url.Scheme}://{request.Url.Authority}{request.ApplicationPath.TrimEnd('/')}/";
        }

        public static System.Uri CreateCompleteUrl(string relativeUrl)
        {
            var completeUrl = GetBaseUrl() + relativeUrl;
            return new System.Uri(completeUrl);
        }
    }
}