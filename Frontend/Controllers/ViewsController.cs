using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Frontend.Controllers
{
    public class ViewsController : Controller
    {
        public ViewsController()
        { }

        [Route("views/{view}")]
        public HttpResponseMessage GetView(string view)
        {
            return CreateResponseMessageFromFile($"~/Views/{view}.html");
        }

        [Route("")]
        public HttpResponseMessage GetIndex()
        {
            return CreateResponseMessageFromFile($"~/Views/index.html");
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

        [Route("rooms")]
        public HttpResponseMessage GetRooms()
        {
            return GetIndex();
        }

        [Route("breadbookings")]
        public HttpResponseMessage GetBreadBookings()
        {
            return GetIndex();
        }

        [Route("accounting")]
        public HttpResponseMessage GetAccounting()
        {
            return GetIndex();
        }

        [Route("users")]
        public HttpResponseMessage GetUsers()
        {
            return GetIndex();
        }

        [Route("login")]
        public HttpResponseMessage GetLogin()
        {
            return GetIndex();
        }

        private HttpResponseMessage CreateResponseMessageFromFile(string fileName)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var fileContent = File.ReadAllText(HttpContext.Current.Server.MapPath(fileName));
            response.Content = new StringContent(fileContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
