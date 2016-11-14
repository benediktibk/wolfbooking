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
        [HttpGet]
        public IList<Bread> GetAllBreads()
        {
            LogInfo("fetching all currently available breads");
            return _bookingFacade.GetCurrentAvailableBreads();
        }

        [Route("api/breads/items")]
        [HttpGet]
        public IList<Bread> GetBreadsById([FromUri] List<int> ids)
        {
            LogInfo($"fetching breads with ids {ids}");
            return _bookingFacade.GetBreads(ids);
        }

        [Route("api/breads/item/{id}")]
        [HttpGet]
        public Bread GetBreadById(int id)
        {
            LogInfo($"fetching bread with id {id}");
            var bread = _bookingFacade.GetBread(id);

            if (bread == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return bread;
        }

        [Route("api/breads")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public HttpResponseMessage CreateBread([FromBody]Bread bread)
        {
            if (bread == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            LogInfo($"creating bread {{{bread}}}");
            var id = _bookingFacade.AddBread(bread);
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/breads/item/{id}");
            return response;
        }

        [Route("api/breads/item/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpPut]
        public HttpResponseMessage UpdateBread(int id, [FromBody]Bread bread)
        {
            if (bread == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            bread.Id = id;
            LogInfo($"updating bread {{{bread}}}");
            return _bookingFacade.UpdateBread(bread) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/breads/item/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpDelete]
        public HttpResponseMessage DeleteBread(int id)
        {
            LogInfo($"deleting bread with id {id}");
            return _bookingFacade.DeleteBread(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
