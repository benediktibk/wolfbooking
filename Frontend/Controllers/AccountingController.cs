using Backend;
using Backend.Facade;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Frontend.Controllers
{
    [Authorize]
    public class AccountingController : Controller
    {
        private BookingFacade _bookingFacade;

        public AccountingController()
        {
            _bookingFacade = Factory.BookingFacade;
        }

        [Route("api/accounting/calculatebill/{room}")]
        [Authorize(Roles = "Managers")]
        [HttpGet]
        public Bill CalculateBill(int room, [FromUri] DateTime startDate, [FromUri] DateTime endDate)
        {
            LogDebug($"calculating bill for room with id {room}");

            var result = _bookingFacade.CalculateBill(room, startDate, endDate);

            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return result;
        }
    }
}