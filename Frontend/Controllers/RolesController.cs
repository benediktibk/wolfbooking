﻿using Backend;
using Backend.Facade;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Backend.Persistence;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Frontend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private BookingFacade _bookingFacade;

        public RolesController(BookingFacade bookingFacade)
        {
            _bookingFacade = bookingFacade;
        }

        [Route("api/roles/")]
        [HttpGet]
        public IList<WolfBookingRole> GetAllRoles()
        {
            LogDebug("fetching all currently available roles");
            return _bookingFacade.GetAllRoles().ToList();
        }

        [Route("api/roles/{username}")]
        [HttpGet]
        [Authorize(Roles = "User")]
        public IList<string> GetRolesForUser(string username)
        {
            var currentUser = RequestContext.Principal.Identity.Name;

            if (!_bookingFacade.IsUserAllowedToSeeDataOfUser(currentUser, username))
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
            }

            return _bookingFacade.GetRoleNamesForUser(username).ToList();
        }
    }
}