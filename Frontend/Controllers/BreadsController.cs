using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;
using System.Net.Http;

namespace Frontend.Controllers
{
    [Authorize]
    public class BreadsController : Controller
    {
        private BookingFacade _bookingFacade;

        public BreadsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/breads/all")]
        [Authorize(Roles = "Users")]
        [HttpGet]
        public IList<Bread> GetAllBreads()
        {
            LogDebug("fetching all currently available breads");
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        [Route("api/breads/items")]
        [Authorize(Roles = "Users")]
        [HttpGet]
        public IList<Bread> GetBreadsById([FromUri] List<int> ids)
        {
            LogDebug($"fetching breads with ids {ids}");
            return _bookingFacade.GetBreads(ids);
        }

        [Route("api/breads/item/{id}")]
        [Authorize(Roles = "Users")]
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
        [Authorize(Roles = "Managers")]
        [HttpPost]
        public HttpResponseMessage CreateBread([FromBody]Bread bread)
        {
            if (bread == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            LogDebug($"creating bread {{{bread}}}");
            var id = _bookingFacade.AddBread(bread);
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/breads/item/{id}");
            return response;
        }

        [Route("api/breads/item/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpPut]
        public HttpResponseMessage UpdateBread(int id, [FromBody]Bread bread)
        {
            if (bread == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            bread.Id = id;
            LogDebug($"updating bread {{{bread}}}");
            return _bookingFacade.UpdateBread(bread) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/breads/item/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpDelete]
        public HttpResponseMessage DeleteBread(int id)
        {
            LogDebug($"deleting bread with id {id}");
            return _bookingFacade.DeleteBread(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
