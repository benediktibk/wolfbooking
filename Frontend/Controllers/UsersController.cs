using Backend;
using Backend.Facade;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Manager,Admin")]
    public class UsersController : Controller
    {
        private readonly BookingFacade _bookingFacade;

        public UsersController(BookingFacade bookingFacade)
        {
            _bookingFacade = bookingFacade;
        }

        [Route("api/users/all")]
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public IList<User> GetAllUsers()
        {
            LogDebug("fetching all currently available users");
            return _bookingFacade.GetAllUsers();
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public User GetUserById(int id)
        {
            LogDebug($"fetching user with id {id}");
            var user = _bookingFacade.GetUser(id);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return user;
        }

        [Route("api/users/username/{username}")]
        [Authorize(Roles = "Users")]
        [HttpGet]
        public User GetUserByUsername(string username)
        {
            LogDebug($"fetching user {username}");

            var currentUser = RequestContext.Principal.Identity.Name;
            if (!_bookingFacade.IsUserAllowedToSeeDataOfUser(currentUser, username))
                throw new HttpResponseException(HttpStatusCode.Forbidden);

            var user = _bookingFacade.GetUserByUsername(username);

            if (user == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return user;
        }

        [Route("api/users")]
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public HttpResponseMessage CreateUser([FromBody]User user, [FromBody]string password)
        {
            if (user == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            LogDebug($"creating user {{{user}}}");
            var id = _bookingFacade.AddUser(user, password);

            if (id < 0)
                return new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict };

            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.Created };
            response.Headers.Location = CreateCompleteUri($"api/users/item/{id}");
            return response;
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public HttpResponseMessage UpdateUser(int id, [FromBody]User user)
        {
            if (user == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            user.Id = id;
            LogDebug($"updating user {{{user}}}");
            return _bookingFacade.UpdateUser(user) ? new HttpResponseMessage(HttpStatusCode.NoContent) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/users/item/{id}")]
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(int id)
        {
            LogDebug($"deleting user with id {id}");
            return _bookingFacade.DeleteUser(id) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}