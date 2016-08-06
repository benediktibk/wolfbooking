using Backend;
using Backend.Facade;
using System.Collections.Generic;
using System.Web.Http;

namespace Frontend.Controllers
{
    public class RolesController : Controller
    {
        private BookingFacade _bookingFacade;

        public RolesController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/roles/all")]
        [HttpGet]
        public IList<Role> GetAllRoles()
        {
            LogDebug("fetching all currently available roles");
            return _bookingFacade.GetAllRoles();
        }

        [Route("api/roles/foruser/{username}")]
        [HttpGet]
        [Authorize(Roles = "Users")]
        public IList<string> GetRolesForUser(string username)
        {
            var currentUser = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeRolesOfUser(currentUser, username))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }

            return _bookingFacade.GetRolesForUser(username);
        }
    }
}