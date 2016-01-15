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
        public HttpResponseMessage GetView(string view)
        {
            return GetFile($"~/Views/{view}.html");
        }

        [Route("")]
        public HttpResponseMessage GetIndex()
        {
            return GetFile($"~/index.html");
        }

        [Route("home")]
        public HttpResponseMessage GetHome()
        {
            return GetIndex();
        }

        [Route("breads")]
        public HttpResponseMessage GetBreads()
        {
            return GetIndex();
        }

        private HttpResponseMessage GetFile(string fileName)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var fileContent = File.ReadAllText(HttpContext.Current.Server.MapPath(fileName));
            response.Content = new StringContent(fileContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }
    }
}
