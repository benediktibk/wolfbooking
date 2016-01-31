using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;
using System.Net.Http;

namespace Frontend.Controllers
{
    //[Authorize]
    public class BreadsController : Controller
    {
        private BookingFacade _bookingFacade;

        public BreadsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/breads/all")]
        [HttpGet]
        public IList<Bread> GetAllBreads()
        {
            LogDebug("fetching all currently available breads");
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        [Route("api/breads/item/{id}")]
        [HttpGet]
        public Bread GetBreadById(int id)
        {
            LogDebug($"fetching bread with id {id}");
            var bread = _bookingFacade.GetBread(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return bread;
        }

        [Route("api/breads")]
        [HttpPost]
        public HttpResponseMessage CreateBread([FromBody]Bread bread)
        {
            LogDebug($"creating bread {{{bread}}}");
            var id = _bookingFacade.AddBread(bread);
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/breads/item/{id}");
            return response;
        }

        [Route("api/breads/item/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateBread(int id, [FromBody]Bread bread)
        {
            bread.Id = id;
            LogDebug($"updating bread {{{bread}}}");
            return _bookingFacade.UpdateBread(bread) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/breads/item/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteBread(int id)
        {
            LogDebug($"deleting bread with id {id}");
            return _bookingFacade.DeleteBread(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
