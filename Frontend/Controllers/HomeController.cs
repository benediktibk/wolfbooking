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
            var content = TemplateEngine.Parse(@"~/Views/Shared/Layout.html", @"~/Views/Home/Index.html", "Home");
            response.Content = new StringContent(content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
