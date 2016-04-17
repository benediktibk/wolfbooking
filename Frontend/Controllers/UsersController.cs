using Backend;
using Backend.Facade;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Frontend.Controllers
{
    public class UsersController : Controller
    {
        private BookingFacade _bookingFacade;

        public UsersController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/users/all")]
        [Authorize(Roles = "Administrators")]
        [HttpGet]
        public IList<User> GetAllUsers()
        {
            LogDebug("fetching all currently available users");
            return _bookingFacade.GetCurrentAvailableUsersWithoutPasswords();
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrators")]
        [HttpGet]
        public User GetUserById(int id)
        {
            LogDebug($"fetching user with id {id}");
            var user = _bookingFacade.GetUser(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return user;
        }

        [Route("api/users")]
        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public HttpResponseMessage CreateUser([FromBody]User user)
        {
            LogDebug($"creating user {{{user}}}");
            var id = _bookingFacade.AddUser(user);

            if (id < 0)
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict };

            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/users/item/{id}");
            return response;
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrators")]
        [HttpPut]
        public HttpResponseMessage UpdateUser(int id, [FromBody]User user)
        {
            user.Id = id;
            LogDebug($"updating user {{{user}}}");
            return _bookingFacade.UpdateUser(user) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrators")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(int id)
        {
            LogDebug($"deleting user with id {id}");
            return _bookingFacade.DeleteUser(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}