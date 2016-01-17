using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;
using System.Net.Http;

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
        [HttpGet]
        public IEnumerable<Bread> GetAllBreads()
        {
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        [Route("api/breads/item/{id}")]
        [HttpGet]
        public Bread GetBreadById(int id)
        {
            var bread = _bookingFacade.GetBread(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return bread;
        }

        [Route("api/breads/item")]
        [HttpPost]
        public HttpResponseMessage UpdateBreadById([FromBody]Bread bread)
        {
            if (bread.Id == 0)
            {
                _bookingFacade.AddBread(bread);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                if (!_bookingFacade.UpdateBread(bread))
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

        }
    }
}
