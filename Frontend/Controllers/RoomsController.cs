﻿using System.Web.Http;
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

        public RoomsController(BookingFacade bookingFacade)
        {
            _bookingFacade = bookingFacade;
        }

        [Route("api/rooms/all")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IList<Room> GetAllRooms()
        {
            LogDebug("fetching all currently available rooms");
            return _bookingFacade.GetCurrentAvailableRooms();
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public Room GetRoomById(int id)
        {
            LogDebug($"fetching room with id {id}");
            var room = _bookingFacade.GetRoom(id);

            if (room == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return room;
        }

        [Route("api/rooms/inuse/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public bool GetInUseById(int id)
        {
            LogDebug($"checking if room with id {id} is in use");
            return _bookingFacade.IsRoomInUse(id);
        }

        [Route("api/rooms")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public HttpResponseMessage CreateRoom([FromBody]Room room)
        {
            if (room == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            LogDebug($"creating room {{{room}}}");
            var id = _bookingFacade.AddRoom(room);
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/rooms/item/{id}");
            return response;
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpPut]
        public HttpResponseMessage UpdateRoom(int id, [FromBody]Room room)
        {
            if (room == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            room.Id = id;
            LogDebug($"updating room {{{room}}}");
            return _bookingFacade.UpdateRoom(room) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/rooms/item/{id}")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpDelete]
        public HttpResponseMessage DeleteRoom(int id)
        {
            LogDebug($"deleting room with id {id}");
            return _bookingFacade.DeleteRoom(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}