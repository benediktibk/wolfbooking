using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using Backend.Business;
using Backend.Persistence;
using System.Net;
using Backend;

namespace Facade.Controllers
{
    public class BreadsController : ApiController
    {
        private static Factory _factory;
        private static BreadFactory _breadFactory;

        static BreadsController()
        {
            _factory = new Factory();
            _breadFactory = _factory.BreadFactory;
        }

        public IEnumerable<Models.Bread> GetCurrentAvailableBreads()
        {
            return _breadFactory.GetCurrentAvailableBreads().Select(x => new Models.Bread(x));
        }

        public Models.Bread GetBread(int id)
        {
            var bread = _breadFactory.Get(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return new Models.Bread(bread);
        }
    }
}
