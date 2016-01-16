using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;

namespace Frontend.Controllers
{
    public class BreadsController : ApiController
    {
        private BookingFacade _bookingFacade;

        public BreadsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/breads/all")]
        public IEnumerable<Bread> GetApiAll()
        {
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        [Route("api/breads/item/{id}")]
        public Bread GetApiItem(int id)
        {
            var bread = _bookingFacade.GetBread(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return bread;
        }
    }
}
