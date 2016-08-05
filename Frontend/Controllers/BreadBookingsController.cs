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
        private BookingFacade _bookingFacade;

        public BreadBookingsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/breadbookings/currentbyroom/{id}")]
        [Authorize(Roles = "Users")]
        [HttpGet]
        public BreadBookings GetCurrentBreadBookingsByRoomId(int id)
        {
            LogDebug($"fetching current bread bookings for room {id}");
            var user = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRoom(user, id))
            {
                LogDebug($"user {user} is not allowed to see this room");
                return null;
            }

            var result = _bookingFacade.GetCurrentBreadBookingsForRoom(id);

            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return result;
        }

        [Route("api/breadbookings/previousbyroom/{id}")]
        [Authorize(Roles = "Users")]
        [HttpGet]
        public List<BreadBookings> GetPreviousBreadBookingsByRoomId(int id)
        {
            LogDebug($"fetching previous bread bookings for room {id}");
            var user = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRoom(user, id))
            {
                LogDebug($"user {user} is not allowed to see this room");
                return null;
            }

            return _bookingFacade.GetPreviousBreadBookingsForRoom(id);
        }

        [Route("api/breadbookings/item/{id}")]
        [Authorize(Roles = "Users")]
        [HttpPut]
        public HttpResponseMessage UpdateBreadBookings(int id, [FromBody]BreadBookings breadBookings)
        {
            if (breadBookings == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            breadBookings.Id = id;
            LogDebug($"updating bread booking list {{{breadBookings}}}");

            var user = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRoom(user, id))
            {
                LogDebug($"user {user} is not allowed to see this room");
                return null;
            }

            return _bookingFacade.UpdateBreadBookings(breadBookings) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}