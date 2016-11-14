using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;
using System.Net.Http;

namespace Frontend.Controllers
{
    [Authorize]
    public class BreadBookingsController : Controller
    {
        private readonly BookingFacade _bookingFacade;

        public BreadBookingsController(BookingFacade bookingFacade)
        {
            _bookingFacade = bookingFacade;
        }

        [Route("api/breadbookings/currentbyroom/{id}")]
        [HttpGet]
        public BreadBookings GetCurrentBreadBookingsByRoomId(int id)
        {
            LogDebug($"fetching current bread bookings for room {id}");
            var user = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRoom(user, id))
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            var result = _bookingFacade.GetCurrentBreadBookingsForRoom(id);

            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return result;
        }

        [Route("api/breadbookings/item/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateBreadBookings(int id, [FromBody]BreadBookings breadBookings)
        {
            if (breadBookings == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            breadBookings.Id = id;
            LogDebug($"updating bread booking list {{{breadBookings}}}");

            var user = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRoom(user, breadBookings.Room))
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            return _bookingFacade.UpdateBreadBookings(breadBookings) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}