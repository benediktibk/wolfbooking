using System.Net.Http;
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
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent($"<html><body>blub</body></html>");
            return response;
        }

        [Route("home/index")]
        public HttpResponseMessage GetIndex()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent($"<html><body>blob</body></html>");
            return response;
        }
    }
}
