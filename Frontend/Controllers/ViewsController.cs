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

        [Route("views/{view}")]
        public HttpResponseMessage Get(string view)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var fileContent = File.ReadAllText(HttpContext.Current.Server.MapPath($"~/Views/{view}.html"));
            response.Content = new StringContent(fileContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
