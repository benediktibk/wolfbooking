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
        public IList<Role> GetAllUsers()
        {
            LogDebug("fetching all currently available roles");
            return _bookingFacade.GetAllRoles();
        }
    }
}