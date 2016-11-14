using System.Web.Http;
using System.Collections.Generic;
using System.Net;
using Backend;
using Backend.Facade;
using System.Net.Http;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoomsController : Controller
    {
        private BookingFacade _bookingFacade;

        public RoomsController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/rooms/all")]
        [Authorize(Roles = "Managers")]
        [HttpGet]
        public IList<Room> GetAllRooms()
        {
            LogInfo("fetching all currently available rooms");
            return _bookingFacade.GetCurrentAvailableRooms();
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpGet]
        public Room GetRoomById(int id)
        {
            LogInfo($"fetching room with id {id}");
            var room = _bookingFacade.GetRoom(id);

            if (room == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return room;
        }

        [Route("api/rooms/inuse/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpGet]
        public bool GetInUseById(int id)
        {
            LogInfo($"checking if room with id {id} is in use");
            return _bookingFacade.IsRoomInUse(id);
        }

        [Route("api/rooms")]
        [Authorize(Roles = "Managers")]
        [HttpPost]
        public HttpResponseMessage CreateRoom([FromBody]Room room)
        {
            if (room == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            LogInfo($"creating room {{{room}}}");
            var id = _bookingFacade.AddRoom(room);
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/rooms/item/{id}");
            return response;
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpPut]
        public HttpResponseMessage UpdateRoom(int id, [FromBody]Room room)
        {
            if (room == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            room.Id = id;
            LogInfo($"updating room {{{room}}}");
            return _bookingFacade.UpdateRoom(room) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Managers")]
        [HttpDelete]
        public HttpResponseMessage DeleteRoom(int id)
        {
            LogInfo($"deleting room with id {id}");
            return _bookingFacade.DeleteRoom(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}