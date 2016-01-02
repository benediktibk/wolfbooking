using RazorEngine;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Facade.Controllers
{
    public class HomeController : ApiController
    {
        public HomeController()
        {

        }

        [Route("home")]
        public HttpResponseMessage Get()
        {
            return GetIndex();
        }

        [Route("home/index")]
        public HttpResponseMessage GetIndex()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var viewPath = HttpContext.Current.Server.MapPath(@"~/Views/Home/Index.cshtml");
            var template = File.ReadAllText(viewPath);
            var parsedView = Razor.Parse(template);
            response.Content = new StringContent(parsedView);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
