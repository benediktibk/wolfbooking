using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;

namespace Frontend.Controllers
{
    public class BreadsController : ApiController
    {
        private Factory _factory;
        private BookingFacade _bookingFacade;

        public BreadsController()
        {
            _factory = new Factory();
            _bookingFacade = _factory.BookingFacade;
        }

        public IEnumerable<Bread> GetCurrentAvailableBreads()
        {
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        public Bread GetBread(int id)
        {
            var bread = _bookingFacade.GetBread(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return bread;
        }
    }
}
