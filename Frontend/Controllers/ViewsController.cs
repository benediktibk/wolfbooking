using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Facade.Controllers
{
    public class ViewsController : ApiController
    {
        public ViewsController()
        {

        }

        [Route("Views/home")]
        public HttpResponseMessage GetHome()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var fileContent = File.ReadAllText(HttpContext.Current.Server.MapPath(@"~/Views/home.html"));
            response.Content = new StringContent(fileContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
