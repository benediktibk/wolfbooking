﻿using System.Net.Http;
using System.Web.Http;

namespace Facade.Controllers
{
    public class HomeController : ApiController
    {
        public HomeController()
        {

        }

        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent("<html><body>blub</body></html>");
            return response;
        }
    }
}
